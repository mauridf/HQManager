using HQManager.Domain.Entities;
using HQManager.Domain.Enums;
using HQManager.Domain.Interfaces;
using MongoDB.Driver;

namespace HQManager.Infra.Data.Repositories;

public class PersonagemRepository : RepositoryBase<Personagem>, IPersonagemRepository
{
    public PersonagemRepository(IMongoCollection<Personagem> collection) : base(collection)
    {
    }

    public async Task<IEnumerable<Personagem>> GetByTipoAsync(TipoPersonagem tipo)
    {
        var filter = Builders<Personagem>.Filter.Eq(p => p.Tipo, tipo);
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<Personagem>> GetByNomeAsync(string nome)
    {
        var filter = Builders<Personagem>.Filter.Regex(p => p.Nome,
            new MongoDB.Bson.BsonRegularExpression(nome, "i")); // "i" para case insensitive
        return await _collection.Find(filter).ToListAsync();
    }
}