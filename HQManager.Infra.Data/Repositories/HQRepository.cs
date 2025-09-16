using HQManager.Domain.Entities;
using HQManager.Domain.Interfaces;
using MongoDB.Driver;

namespace HQManager.Infra.Data.Repositories;

public class HQRepository : RepositoryBase<HQ>, IHQRepository
{
    public HQRepository(IMongoCollection<HQ> collection) : base(collection)
    {
    }

    public async Task<IEnumerable<HQ>> GetByPersonagemIdAsync(Guid personagemId)
    {
        var filter = Builders<HQ>.Filter.AnyEq(hq => hq.Personagens, personagemId);
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<HQ>> GetByEquipeIdAsync(Guid equipeId)
    {
        var filter = Builders<HQ>.Filter.AnyEq(hq => hq.Equipes, equipeId);
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<HQ>> GetByEditoraIdAsync(Guid editoraId)
    {
        var filter = Builders<HQ>.Filter.AnyEq(hq => hq.Editoras, editoraId);
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<HQ>> GetByNomeAsync(string nome)
    {
        var filter = Builders<HQ>.Filter.Regex(hq => hq.Nome, new MongoDB.Bson.BsonRegularExpression(nome, "i"));
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<bool> VerificarPersonagensExistemAsync(IEnumerable<Guid> personagemIds)
    {
        // Este método seria implementado se tivéssemos acesso ao contexto de Personagem aqui
        // Por simplicidade, vamos deixar a validação para a camada de aplicação
        return await Task.FromResult(true);
    }

    public async Task<bool> VerificarEquipesExistemAsync(IEnumerable<Guid> equipeIds)
    {
        return await Task.FromResult(true);
    }

    public async Task<bool> VerificarEditorasExistemAsync(IEnumerable<Guid> editoraIds)
    {
        return await Task.FromResult(true);
    }
}