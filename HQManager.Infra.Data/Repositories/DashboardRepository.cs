using HQManager.Domain.DTOs;
using HQManager.Domain.Entities;
using HQManager.Domain.Enums;
using HQManager.Domain.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HQManager.Infra.Data.Repositories;

public class DashboardRepository : IDashboardRepository
{
    private readonly IMongoCollection<HQ> _hqsCollection;
    private readonly IMongoCollection<Personagem> _personagensCollection;
    private readonly IMongoCollection<Equipe> _equipesCollection;
    private readonly IMongoCollection<Edicao> _edicoesCollection;
    private readonly IMongoCollection<Editora> _editorasCollection;

    public DashboardRepository(IMongoDatabase database)
    {
        _hqsCollection = database.GetCollection<HQ>("HQs");
        _personagensCollection = database.GetCollection<Personagem>("Personagens");
        _equipesCollection = database.GetCollection<Equipe>("Equipes");
        _edicoesCollection = database.GetCollection<Edicao>("Edicoes");
        _editorasCollection = database.GetCollection<Editora>("Editoras");
    }

    public async Task<int> ObterTotalHQsAsync()
    {
        return (int)await _hqsCollection.CountDocumentsAsync(_ => true);
    }

    public async Task<int> ObterTotalPersonagensAsync()
    {
        return (int)await _personagensCollection.CountDocumentsAsync(_ => true);
    }

    public async Task<int> ObterTotalEquipesAsync()
    {
        return (int)await _equipesCollection.CountDocumentsAsync(_ => true);
    }

    public async Task<int> ObterTotalEdicoesLidasAsync()
    {
        return (int)await _edicoesCollection.CountDocumentsAsync(e => e.Lida);
    }

    public async Task<double?> ObterNotaMediaAsync()
    {
        try
        {
            var pipeline = new[]
            {
            // Filtra apenas edições que têm nota (não nula)
            BsonDocument.Parse(@"{
                $match: {
                    Nota: { $ne: null, $exists: true }
                }
            }"),
            // Agrupa e calcula a média
            BsonDocument.Parse(@"{
                $group: {
                    _id: null,
                    media: { $avg: '$Nota' }
                }
            }")
        };

            var result = await _edicoesCollection.AggregateAsync<BsonDocument>(pipeline);
            var mediaDoc = await result.FirstOrDefaultAsync();

            // Se não houver documentos com nota, retorna null
            if (mediaDoc == null || !mediaDoc.Contains("media") || mediaDoc["media"].IsBsonNull)
            {
                return null;
            }

            return Math.Round(mediaDoc["media"].AsDouble, 1);
        }
        catch (Exception ex)
        {
            // Log do erro (em produção, use ILogger)
            Console.WriteLine($"Erro ao calcular nota média: {ex.Message}");
            return null;
        }
    }

    public async Task<List<HQ>> ObterHQsRecentesAsync(int limite = 4)
    {
        return await _hqsCollection
            .Find(_ => true)
            .SortByDescending(hq => hq.CriadoEm)
            .Limit(limite)
            .ToListAsync();
    }

    public async Task<HQsPorEditoraResponse> ObterHQsPorEditoraAsync()
    {
        var response = new HQsPorEditoraResponse();

        // Obter todas as editoras
        var editoras = await _editorasCollection.Find(_ => true).ToListAsync();
        var todasHQs = await _hqsCollection.Find(_ => true).ToListAsync();
        var todasEdicoes = await _edicoesCollection.Find(_ => true).ToListAsync();

        foreach (var editora in editoras)
        {
            // HQs desta editora
            var hqsDaEditora = todasHQs
                .Where(hq => hq.Editoras.Contains(editora.Id))
                .ToList();

            // Edições das HQs desta editora
            var edicoesDaEditora = todasEdicoes
                .Where(e => hqsDaEditora.Any(hq => hq.Id == e.HqId))
                .ToList();

            response.Estatisticas.Add(new EditoraEstatistica
            {
                EditoraId = editora.Id,
                EditoraNome = editora.Nome,
                TotalHQs = hqsDaEditora.Count,
                HQsEmAndamento = hqsDaEditora.Count(hq => hq.Status == StatusHQ.EmAndamento),
                HQsFinalizadas = hqsDaEditora.Count(hq => hq.Status == StatusHQ.Finalizado),
                HQsCanceladas = hqsDaEditora.Count(hq => hq.Status == StatusHQ.Cancelado),
                HQsIncompletas = hqsDaEditora.Count(hq => hq.Status == StatusHQ.Incompleto),
                TotalEdicoes = edicoesDaEditora.Count,
                EdicoesLidas = edicoesDaEditora.Count(e => e.Lida)
            });
        }

        // Ordenar por nome da editora
        response.Estatisticas = response.Estatisticas
            .OrderBy(e => e.EditoraNome)
            .ToList();

        return response;
    }
}