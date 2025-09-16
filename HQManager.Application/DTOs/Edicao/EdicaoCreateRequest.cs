using System.ComponentModel.DataAnnotations;

namespace HQManager.Application.DTOs.Edicao;

public class EdicaoCreateRequest
{
    [Required(ErrorMessage = "O campo HqId é obrigatório.")]
    public Guid HqId { get; set; }

    [MaxLength(200, ErrorMessage = "O título não pode exceder 200 caracteres.")]
    public string? Titulo { get; set; }

    [Required(ErrorMessage = "O campo Número é obrigatório.")]
    [MaxLength(20, ErrorMessage = "O número não pode exceder 20 caracteres.")]
    public string Numero { get; set; } = string.Empty;

    [MaxLength(1000, ErrorMessage = "A sinopse não pode exceder 1000 caracteres.")]
    public string? Sinopse { get; set; }

    [Url(ErrorMessage = "A capa deve ser uma URL válida.")]
    public string? Capa { get; set; }

    public bool Lida { get; set; } = false;

    [MaxLength(500, ErrorMessage = "As observações não podem exceder 500 caracteres.")]
    public string? Obs { get; set; }

    [Range(0, 10, ErrorMessage = "A nota deve estar entre 0 e 10.")]
    public int? Nota { get; set; }

    public DateTime? DataLeitura { get; set; }
}