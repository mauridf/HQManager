using HQManager.Application.DTOs.HQ;

namespace HQManager.Application.DTOs.Dashboard;

public class DashboardResponse
{
    public required EstatisticasResponse Estatisticas { get; set; }
    public required List<HQResumoResponse> HQsRecentes { get; set; }
}

public class EstatisticasResponse
{
    public int TotalHQs { get; set; }
    public int TotalPersonagens { get; set; }
    public int TotalEquipes { get; set; }
    public int TotalEdicoesLidas { get; set; }
    public double? NotaMedia { get; set; }
}

public class HQResumoResponse
{
    public Guid Id { get; set; }
    public required string Nome { get; set; }
    public required string Progresso { get; set; } // Formato: "5/12"
    public required string Status { get; set; }
    public int EdicoesLidas { get; set; }
    public int TotalEdicoes { get; set; }
}