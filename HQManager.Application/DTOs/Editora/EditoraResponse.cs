namespace HQManager.Application.DTOs.Editora;

public class EditoraResponse
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int? AnoCriacao { get; set; }
    public string? Logotipo { get; set; }
    public string? PaisOrigem { get; set; }
    public string? SiteOficial { get; set; }
}