using HQManager.Application.DTOs.Auth;
using HQManager.Application.DTOs.Usuario;

namespace HQManager.CrossCutting.Services;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(string email, string senha);
    // Futuramente: Task<bool> AlterarSenhaAsync(Guid usuarioId, string senhaAtual, string novaSenha);
}