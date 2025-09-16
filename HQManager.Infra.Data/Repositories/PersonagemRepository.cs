using HQManager.Domain.Entities;
using HQManager.Domain.Interfaces;
using MongoDB.Driver;

namespace HQManager.Infra.Data.Repositories;

public class PersonagemRepository : RepositoryBase<Personagem>, IPersonagemRepository
{
    public PersonagemRepository(IMongoCollection<Personagem> collection) : base(collection)
    {
    }
    // Implementar métodos específicos aqui no futuro
}