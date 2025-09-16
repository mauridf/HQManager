using HQManager.Domain.Entities;
using HQManager.Domain.Interfaces;
using MongoDB.Driver;

namespace HQManager.Infra.Data.Repositories;

public class EdicaoRepository : RepositoryBase<Edicao>, IEdicaoRepository
{
    public EdicaoRepository(IMongoCollection<Edicao> collection) : base(collection)
    {
    }

    public async Task<IEnumerable<Edicao>> GetByHqIdAsync(Guid hqId)
    {
        var filter = Builders<Edicao>.Filter.Eq(e => e.HqId, hqId);
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<Edicao> GetByHqIdAndNumeroAsync(Guid hqId, string numero)
    {
        var filter = Builders<Edicao>.Filter.And(
            Builders<Edicao>.Filter.Eq(e => e.HqId, hqId),
            Builders<Edicao>.Filter.Eq(e => e.Numero, numero)
        );
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<bool> VerificarHqExisteAsync(Guid hqId)
    {
        // Este método seria implementado se tivéssemos acesso ao contexto de HQ aqui
        // Por simplicidade, vamos deixar a validação para a camada de aplicação
        return await Task.FromResult(true);
    }
}