namespace HQManager.Application.DTOs.Equipe;

public class EquipeResponse
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string? Imagem { get; set; }
    public int? AnoCriacao { get; set; }
}