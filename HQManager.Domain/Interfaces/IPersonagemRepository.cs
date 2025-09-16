using HQManager.Domain.Entities;

namespace HQManager.Domain.Interfaces;

public interface IPersonagemRepository : IRepositoryBase<Personagem>
{
    // Métodos específicos para Personagem podem ser adicionados aqui no futuro
    // Task<IEnumerable<Personagem>> GetByTipoAsync(TipoPersonagem tipo);
    // Task<IEnumerable<Personagem>> GetByNomeAsync(string nome);
}