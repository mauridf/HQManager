using HQManager.Domain.Entities;

namespace HQManager.Domain.Interfaces;

public interface IEquipeRepository : IRepositoryBase<Equipe>
{
    Task<IEnumerable<Equipe>> GetByNomeAsync(string nome);
}