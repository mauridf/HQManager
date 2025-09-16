using HQManager.Domain.Enums;

namespace HQManager.Application.DTOs.Personagem;

public class PersonagemResponse
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public TipoPersonagem Tipo { get; set; }
    public string? Descricao { get; set; }
    public string? Imagem { get; set; }
    public string? PrimeiraAparicao { get; set; }
}