using System.ComponentModel.DataAnnotations;
using HQManager.Domain.Enums;

namespace HQManager.Application.DTOs.Personagem;

public class PersonagemCreateRequest
{
    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    [MaxLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo Tipo é obrigatório.")]
    public TipoPersonagem Tipo { get; set; }

    [MaxLength(500, ErrorMessage = "A descrição não pode exceder 500 caracteres.")]
    public string? Descricao { get; set; }

    [Url(ErrorMessage = "A imagem deve ser uma URL válida.")]
    public string? Imagem { get; set; }

    [MaxLength(100, ErrorMessage = "A primeira aparição não pode exceder 100 caracteres.")]
    public string? PrimeiraAparicao { get; set; }
}