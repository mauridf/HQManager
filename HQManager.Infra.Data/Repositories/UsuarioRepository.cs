using HQManager.Domain.Entities;
using HQManager.Domain.Interfaces;
using MongoDB.Driver;

namespace HQManager.Infra.Data.Repositories;

public class UsuarioRepository : RepositoryBase<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(IMongoCollection<Usuario> collection) : base(collection)
    {
    }

    public async Task<Usuario> GetByEmailAsync(string email)
    {
        var filter = Builders<Usuario>.Filter.Eq(u => u.Email, email);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<bool> EmailEmUsoAsync(string email)
    {
        var filter = Builders<Usuario>.Filter.Eq(u => u.Email, email);
        return await _collection.Find(filter).AnyAsync();
    }
}