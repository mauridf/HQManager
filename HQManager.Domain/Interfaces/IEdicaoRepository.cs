using HQManager.Domain.Entities;

namespace HQManager.Domain.Interfaces;

public interface IEdicaoRepository : IRepositoryBase<Edicao>
{
    Task<IEnumerable<Edicao>> GetByHqIdAsync(Guid hqId);
    Task<Edicao> GetByHqIdAndNumeroAsync(Guid hqId, string numero);
    Task<bool> VerificarHqExisteAsync(Guid hqId);
}