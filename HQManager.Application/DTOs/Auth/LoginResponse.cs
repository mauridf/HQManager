using HQManager.Application.DTOs.Usuario;

namespace HQManager.Application.DTOs.Auth;

public class LoginResponse
{
    public bool Sucesso { get; set; }
    public string? Token { get; set; }
    public DateTime? DataExpiracao { get; set; }
    public string? Mensagem { get; set; }
    public UsuarioResponse? Usuario { get; set; } // Reutiliza o DTO de usuário

    public static LoginResponse Falha(string mensagem) => new LoginResponse { Sucesso = false, Mensagem = mensagem };
    public static LoginResponse CriarSucesso(string token, DateTime dataExpiracao, UsuarioResponse usuario) => new LoginResponse { Sucesso = true, Token = token, DataExpiracao = dataExpiracao, Usuario = usuario };
}