using System.ComponentModel.DataAnnotations;

namespace HQManager.Application.DTOs.Usuario;

public class UsuarioCreateRequest
{
    [Required(ErrorMessage = "O campo Email é obrigatório.")]
    [EmailAddress(ErrorMessage = "O campo Email deve conter um endereço de email válido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo Senha é obrigatório.")]
    [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres.")]
    public string Senha { get; set; } = string.Empty;

    [Required(ErrorMessage = "A confirmação de senha é obrigatória.")]
    [Compare("Senha", ErrorErrorMessage = "A senha e a confirmação de senha não coincidem.")]
    public string ConfirmarSenha { get; set; } = string.Empty;
}