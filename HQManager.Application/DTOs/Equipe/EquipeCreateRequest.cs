using System.ComponentModel.DataAnnotations;

namespace HQManager.Application.DTOs.Equipe;

public class EquipeCreateRequest
{
    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    [MaxLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [MaxLength(500, ErrorMessage = "A descrição não pode exceder 500 caracteres.")]
    public string? Descricao { get; set; }

    [Url(ErrorMessage = "A imagem deve ser uma URL válida.")]
    public string? Imagem { get; set; }

    [Range(1800, 2100, ErrorMessage = "O ano de criação deve ser entre 1800 e 2100.")]
    public int? AnoCriacao { get; set; }
}