using HQManager.Domain.Enums;

namespace HQManager.Domain.Entities;

public class Personagem : EntityBase
{
    public string Nome { get; set; } = string.Empty;
    public TipoPersonagem Tipo { get; set; }
    public string? Descricao { get; set; }
    public string? Imagem { get; set; }
    public string? PrimeiraAparicao { get; set; }
}