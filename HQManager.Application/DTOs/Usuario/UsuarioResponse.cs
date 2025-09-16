namespace HQManager.Application.DTOs.Usuario;

public class UsuarioResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime CriadoEm { get; set; }
    public DateTime? UltimoLogin { get; set; }
}