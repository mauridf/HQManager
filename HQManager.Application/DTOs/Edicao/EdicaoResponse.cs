namespace HQManager.Application.DTOs.Edicao;

public class EdicaoResponse
{
    public Guid Id { get; set; }
    public Guid HqId { get; set; }
    public string? Titulo { get; set; }
    public string Numero { get; set; } = string.Empty;
    public string? Sinopse { get; set; }
    public string? Capa { get; set; }
    public bool Lida { get; set; }
    public string? Obs { get; set; }
    public int? Nota { get; set; }
    public DateTime? DataLeitura { get; set; }
}