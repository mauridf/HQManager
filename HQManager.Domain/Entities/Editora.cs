namespace HQManager.Domain.Entities;

public class Editora : EntityBase
{
    public string Nome { get; set; } = string.Empty;
    public int? AnoCriacao { get; set; }
    public string? Logotipo { get; set; }
    public string? PaisOrigem { get; set; }
    public string? SiteOficial { get; set; }
}