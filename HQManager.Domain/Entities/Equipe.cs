namespace HQManager.Domain.Entities;

public class Equipe : EntityBase
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string? Imagem { get; set; }
    public int? AnoCriacao { get; set; }
}