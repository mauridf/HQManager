using HQManager.Domain.Enums;

namespace HQManager.Application.DTOs.HQ;

public class HQResponse
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? NomeOriginal { get; set; }
    public TipoPublicacao TipoPublicacao { get; set; }
    public StatusHQ Status { get; set; }
    public int? TotalEdicoes { get; set; }
    public string Sinopse { get; set; } = string.Empty;
    public int? AnoLancamento { get; set; }
    public List<Guid> LeiturasRecomendadas { get; set; } = new();
    public string? Imagem { get; set; }
    public List<string> Tags { get; set; } = new();
    public DateTime CriadoEm { get; set; }

    // Arrays de relacionamentos
    public List<Guid> Personagens { get; set; } = new();
    public List<Guid> Equipes { get; set; } = new();
    public List<Guid> Editoras { get; set; } = new();
}