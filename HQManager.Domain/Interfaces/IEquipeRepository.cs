using HQManager.Domain.Entities;

namespace HQManager.Domain.Interfaces;

public interface IEquipeRepository : IRepositoryBase<Equipe>
{
    // Métodos específicos para Equipe podem ser adicionados aqui no futuro
    // Task<IEnumerable<Equipe>> GetByNomeAsync(string nome);
}