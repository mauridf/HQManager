using HQManager.Domain.Entities;
using HQManager.Domain.Interfaces;
using MongoDB.Driver;

namespace HQManager.Infra.Data.Repositories;

public class EquipeRepository : RepositoryBase<Equipe>, IEquipeRepository
{
    public EquipeRepository(IMongoCollection<Equipe> collection) : base(collection)
    {
    }

    public async Task<IEnumerable<Equipe>> GetByNomeAsync(string nome)
    {
        var filter = Builders<Equipe>.Filter.Regex(e => e.Nome,
            new MongoDB.Bson.BsonRegularExpression(nome, "i")); // "i" para case insensitive
        return await _collection.Find(filter).ToListAsync();
    }
}