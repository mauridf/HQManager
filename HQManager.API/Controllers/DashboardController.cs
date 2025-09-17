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
            // Executar consultas de forma explícita (sem Task.WhenAll para evitar problemas de inferência)
            var totalHQsTask = _dashboardRepository.ObterTotalHQsAsync();
            var totalPersonagensTask = _dashboardRepository.ObterTotalPersonagensAsync();
            var totalEquipesTask = _dashboardRepository.ObterTotalEquipesAsync();
            var totalEdicoesLidasTask = _dashboardRepository.ObterTotalEdicoesLidasAsync();
            var notaMediaTask = _dashboardRepository.ObterNotaMediaAsync();
            var hqsRecentesTask = _dashboardRepository.ObterHQsRecentesAsync(4);

            // Aguardar todas as tarefas
            var totalHQs = await totalHQsTask;
            var totalPersonagens = await totalPersonagensTask;
            var totalEquipes = await totalEquipesTask;
            var totalEdicoesLidas = await totalEdicoesLidasTask;
            var notaMedia = await notaMediaTask;
            var hqsRecentes = await hqsRecentesTask;

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
                // Obter edições de forma assíncrona
                var edicoes = await _edicaoRepository.GetByHqIdAsync(hq.Id);

                // Converter para lista para evitar múltiplas enumerações
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

            var response = new DashboardResponse
            {
                Estatisticas = estatisticas,
                HQsRecentes = hqsResumo
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao obter dados do dashboard", error = ex.Message });
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