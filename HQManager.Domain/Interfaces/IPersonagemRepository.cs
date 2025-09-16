using HQManager.Domain.Entities;
using HQManager.Domain.Enums;

namespace HQManager.Domain.Interfaces;

public interface IPersonagemRepository : IRepositoryBase<Personagem>
{
    Task<IEnumerable<Personagem>> GetByTipoAsync(TipoPersonagem tipo);
    Task<IEnumerable<Personagem>> GetByNomeAsync(string nome);
}