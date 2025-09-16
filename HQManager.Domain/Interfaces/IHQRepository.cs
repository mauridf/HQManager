using HQManager.Domain.Entities;

namespace HQManager.Domain.Interfaces;

public interface IHQRepository : IRepositoryBase<HQ>
{
    // Métodos específicos para HQ
    Task<IEnumerable<HQ>> GetByPersonagemIdAsync(Guid personagemId);
    Task<IEnumerable<HQ>> GetByEquipeIdAsync(Guid equipeId);
    Task<IEnumerable<HQ>> GetByEditoraIdAsync(Guid editoraId);
    Task<IEnumerable<HQ>> GetByNomeAsync(string nome);
    Task<bool> VerificarPersonagensExistemAsync(IEnumerable<Guid> personagemIds);
    Task<bool> VerificarEquipesExistemAsync(IEnumerable<Guid> equipeIds);
    Task<bool> VerificarEditorasExistemAsync(IEnumerable<Guid> editoraIds);
}