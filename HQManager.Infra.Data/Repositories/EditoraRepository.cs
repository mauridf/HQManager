using HQManager.Domain.Entities;
using HQManager.Domain.Interfaces;
using MongoDB.Driver;

namespace HQManager.Infra.Data.Repositories;

public class EditoraRepository : RepositoryBase<Editora>, IEditoraRepository
{
    public EditoraRepository(IMongoCollection<Editora> collection) : base(collection)
    {
    }
    // Implementar métodos específicos aqui no futuro
}