namespace HQManager.Domain.DTOs;

public class HQsPorEditoraResponse
{
    public List<EditoraEstatistica> Estatisticas { get; set; } = new();
}

public class EditoraEstatistica
{
    public string EditoraNome { get; set; } = string.Empty;
    public Guid EditoraId { get; set; }
    public int TotalHQs { get; set; }
    public int HQsEmAndamento { get; set; }
    public int HQsFinalizadas { get; set; }
    public int HQsCanceladas { get; set; }
    public int HQsIncompletas { get; set; }
    public int TotalEdicoes { get; set; }
    public int EdicoesLidas { get; set; }
}