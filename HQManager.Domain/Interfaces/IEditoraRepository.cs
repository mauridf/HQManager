using HQManager.Domain.Entities;

namespace HQManager.Domain.Interfaces;

public interface IEditoraRepository : IRepositoryBase<Editora>
{
    // Métodos específicos para Editora podem ser adicionados aqui no futuro
    // Task<Editora> GetByNomeAsync(string nome);
}