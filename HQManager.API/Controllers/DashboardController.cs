using HQManager.Application.DTOs.Dashboard;
using HQManager.Domain.DTOs;
using HQManager.Domain.Entities;
using HQManager.Domain.Enums;
using HQManager.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HQManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IDashboardRepository _dashboardRepository;
    private readonly IEdicaoRepository _edicaoRepository;

    public DashboardController(IDashboardRepository dashboardRepository, IEdicaoRepository edicaoRepository)
    {
        _dashboardRepository = dashboardRepository;
        _edicaoRepository = edicaoRepository;
    }

    [HttpGet]
    public async Task<ActionResult<DashboardResponse>> ObterDadosDashboard()
    {
        try
        {
            // Executar consultas de forma explícita com tratamento individual
            int totalHQs = 0;
            int totalPersonagens = 0;
            int totalEquipes = 0;
            int totalEdicoesLidas = 0;
            double? notaMedia = null;
            List<HQ> hqsRecentes = new();

            try { totalHQs = await _dashboardRepository.ObterTotalHQsAsync(); }
            catch (Exception ex) { Console.WriteLine($"Erro TotalHQs: {ex.Message}"); }

            try { totalPersonagens = await _dashboardRepository.ObterTotalPersonagensAsync(); }
            catch (Exception ex) { Console.WriteLine($"Erro TotalPersonagens: {ex.Message}"); }

            try { totalEquipes = await _dashboardRepository.ObterTotalEquipesAsync(); }
            catch (Exception ex) { Console.WriteLine($"Erro TotalEquipes: {ex.Message}"); }

            try { totalEdicoesLidas = await _dashboardRepository.ObterTotalEdicoesLidasAsync(); }
            catch (Exception ex) { Console.WriteLine($"Erro TotalEdicoesLidas: {ex.Message}"); }

            try { notaMedia = await _dashboardRepository.ObterNotaMediaAsync(); }
            catch (Exception ex) { Console.WriteLine($"Erro NotaMedia: {ex.Message}"); }

            try { hqsRecentes = await _dashboardRepository.ObterHQsRecentesAsync(4); }
            catch (Exception ex) { Console.WriteLine($"Erro HQsRecentes: {ex.Message}"); }

            var estatisticas = new EstatisticasResponse
            {
                TotalHQs = totalHQs,
                TotalPersonagens = totalPersonagens,
                TotalEquipes = totalEquipes,
                TotalEdicoesLidas = totalEdicoesLidas,
                NotaMedia = notaMedia
            };

            var hqsResumo = new List<HQResumoResponse>();

            // Processar cada HQ recente
            foreach (var hq in hqsRecentes)
            {
                try
                {
                    var edicoes = await _edicaoRepository.GetByHqIdAsync(hq.Id);
                    var edicoesList = edicoes.ToList();

                    var edicoesLidas = edicoesList.Count(e => e.Lida);
                    var totalEdicoes = edicoesList.Count;

                    var progresso = totalEdicoes > 0
                        ? $"{edicoesLidas}/{totalEdicoes}"
                        : "0/0";

                    hqsResumo.Add(new HQResumoResponse
                    {
                        Id = hq.Id,
                        Nome = hq.Nome,
                        Progresso = progresso,
                        Status = ObterStatusHQ(hq.Status, edicoesLidas, totalEdicoes),
                        EdicoesLidas = edicoesLidas,
                        TotalEdicoes = totalEdicoes
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro processando HQ {hq.Nome}: {ex.Message}");
                    // Continua processando as outras HQs
                }
            }

            var response = new DashboardResponse
            {
                Estatisticas = estatisticas,
                HQsRecentes = hqsResumo
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Erro ao obter dados do dashboard",
                error = ex.Message,
                stackTrace = ex.StackTrace
            });
        }
    }

    [HttpGet("estatisticas-editoras")]
    public async Task<ActionResult<HQsPorEditoraResponse>> ObterEstatisticasPorEditora()
    {
        try
        {
            var estatisticas = await _dashboardRepository.ObterHQsPorEditoraAsync();

            if (estatisticas == null || estatisticas.Estatisticas.Count == 0)
            {
                return Ok(new HQsPorEditoraResponse
                {
                    Estatisticas = new List<EditoraEstatistica>()
                });
            }

            return Ok(estatisticas);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Erro ao obter estatísticas por editora",
                error = ex.Message
            });
        }
    }

    private static string ObterStatusHQ(StatusHQ statusOriginal, int edicoesLidas, int totalEdicoes)
    {
        if (totalEdicoes == 0)
            return "Não Iniciada";

        if (edicoesLidas == totalEdicoes)
            return "Completo";

        // Converte o enum StatusHQ para string descritiva
        return statusOriginal switch
        {
            StatusHQ.EmAndamento => "Em Andamento",
            StatusHQ.Finalizado => "Finalizado",
            StatusHQ.Cancelado => "Cancelado",
            StatusHQ.Incompleto => "Incompleto",
            _ => statusOriginal.ToString()
        };
    }
}