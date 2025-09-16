namespace HQManager.Domain.Entities;

public class Edicao : EntityBase
{
    public Guid HqId { get; set; } // FK para a HQ
    public string? Titulo { get; set; }
    public string Numero { get; set; } = string.Empty; // "1", "1 de 6", "Única"
    public string? Sinopse { get; set; }
    public string? Capa { get; set; }
    public bool Lida { get; set; } = false;
    public string? Obs { get; set; }
    public int? Nota { get; set; } // 0 - 10
    public DateTime? DataLeitura { get; set; }
}