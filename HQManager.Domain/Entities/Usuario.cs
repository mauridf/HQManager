namespace HQManager.Domain.Entities;

public class Usuario : EntityBase
{
    public string Email { get; set; } = string.Empty;
    public string SenhaHash { get; set; } = string.Empty;
    public DateTime CriadoEm { get; set; }
    public DateTime? UltimoLogin { get; set; }
}