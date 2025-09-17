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
        var pipeline = new[]
        {
            new BsonDocument("$match", new BsonDocument("Nota", new BsonDocument("$ne", null))),
            new BsonDocument("$group", new BsonDocument
            {
                { "_id", null },
                { "media", new BsonDocument("$avg", "$Nota") }
            })
        };

        var result = await _edicoesCollection.AggregateAsync<BsonDocument>(pipeline);
        var media = await result.FirstOrDefaultAsync();

        return media != null && media.Contains("media") ? media["media"].AsDouble : null;
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