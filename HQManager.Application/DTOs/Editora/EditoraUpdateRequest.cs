using System.ComponentModel.DataAnnotations;

namespace HQManager.Application.DTOs.Editora;

public class EditoraUpdateRequest
{
    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    [MaxLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Range(1800, 2100, ErrorMessage = "O ano de criação deve ser entre 1800 e 2100.")]
    public int? AnoCriacao { get; set; }

    [Url(ErrorMessage = "O logotipo deve ser uma URL válida.")]
    public string? Logotipo { get; set; }

    [MaxLength(50, ErrorMessage = "O país de origem não pode exceder 50 caracteres.")]
    public string? PaisOrigem { get; set; }

    [Url(ErrorMessage = "O site oficial deve ser uma URL válida.")]
    public string? SiteOficial { get; set; }
}