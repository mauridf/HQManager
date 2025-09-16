using System.ComponentModel.DataAnnotations;

namespace HQManager.Application.DTOs.Auth;

public class LoginRequest
{
    [Required(ErrorMessage = "O campo Email é obrigatório.")]
    [EmailAddress(ErrorMessage = "O campo Email deve conter um endereço de email válido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo Senha é obrigatório.")]
    public string Senha { get; set; } = string.Empty;
}