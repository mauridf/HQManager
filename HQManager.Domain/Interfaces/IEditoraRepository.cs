using HQManager.Domain.Entities;

namespace HQManager.Domain.Interfaces;

public interface IEditoraRepository : IRepositoryBase<Editora>
{
    Task<Editora> GetByNomeAsync(string nome);
}