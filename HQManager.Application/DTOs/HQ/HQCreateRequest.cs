using System.ComponentModel.DataAnnotations;
using HQManager.Domain.Enums;

namespace HQManager.Application.DTOs.HQ;

public class HQCreateRequest
{
    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    [MaxLength(200, ErrorMessage = "O nome não pode exceder 200 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [MaxLength(200, ErrorMessage = "O nome original não pode exceder 200 caracteres.")]
    public string? NomeOriginal { get; set; }

    [Required(ErrorMessage = "O campo TipoPublicacao é obrigatório.")]
    public TipoPublicacao TipoPublicacao { get; set; }

    [Required(ErrorMessage = "O campo Status é obrigatório.")]
    public StatusHQ Status { get; set; }

    [Range(1, 1000, ErrorMessage = "O total de edições deve ser entre 1 e 1000.")]
    public int? TotalEdicoes { get; set; }

    [Required(ErrorMessage = "O campo Sinopse é obrigatório.")]
    [MaxLength(2000, ErrorMessage = "A sinopse não pode exceder 2000 caracteres.")]
    public string Sinopse { get; set; } = string.Empty;

    [Range(1800, 2100, ErrorMessage = "O ano de lançamento deve ser entre 1800 e 2100.")]
    public int? AnoLancamento { get; set; }

    public List<Guid>? LeiturasRecomendadas { get; set; } = new();

    [Url(ErrorMessage = "A imagem deve ser uma URL válida.")]
    public string? Imagem { get; set; }

    public List<string> Tags { get; set; } = new();

    // Arrays de relacionamentos
    public List<Guid> Personagens { get; set; } = new();
    public List<Guid> Equipes { get; set; } = new();
    public List<Guid> Editoras { get; set; } = new();
}