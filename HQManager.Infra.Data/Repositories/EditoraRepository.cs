using HQManager.Domain.Entities;
using HQManager.Domain.Interfaces;
using MongoDB.Driver;

namespace HQManager.Infra.Data.Repositories;

public class EditoraRepository : RepositoryBase<Editora>, IEditoraRepository
{
    public EditoraRepository(IMongoCollection<Editora> collection) : base(collection)
    {
    }
    public async Task<Editora> GetByNomeAsync(string nome)
    {
        var filter = Builders<Editora>.Filter.Eq(e => e.Nome, nome);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }
}