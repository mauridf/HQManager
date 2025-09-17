using HQManager.Domain.Entities;
using HQManager.Domain.DTOs;

namespace HQManager.Domain.Interfaces;

public interface IDashboardRepository
{
    Task<int> ObterTotalHQsAsync();
    Task<int> ObterTotalPersonagensAsync();
    Task<int> ObterTotalEquipesAsync();
    Task<int> ObterTotalEdicoesLidasAsync();
    Task<double?> ObterNotaMediaAsync();
    Task<List<HQ>> ObterHQsRecentesAsync(int limite = 4);
    Task<HQsPorEditoraResponse> ObterHQsPorEditoraAsync();
}