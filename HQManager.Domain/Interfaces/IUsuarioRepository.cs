using HQManager.Domain.Entities;

namespace HQManager.Domain.Interfaces;

public interface IUsuarioRepository : IRepositoryBase<Usuario>
{
    Task<Usuario> GetByEmailAsync(string email);
    Task<bool> EmailEmUsoAsync(string email);
}