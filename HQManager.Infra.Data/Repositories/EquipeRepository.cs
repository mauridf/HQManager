using HQManager.Domain.Entities;
using HQManager.Domain.Interfaces;
using MongoDB.Driver;

namespace HQManager.Infra.Data.Repositories;

public class EquipeRepository : RepositoryBase<Equipe>, IEquipeRepository
{
    public EquipeRepository(IMongoCollection<Equipe> collection) : base(collection)
    {
    }
    // Implementar métodos específicos aqui no futuro
}