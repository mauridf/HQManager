using HQManager.Application.DTOs.Edicao;
using HQManager.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HQManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EdicoesController : ControllerBase
{
    private readonly IEdicaoRepository _edicaoRepository;
    private readonly IHQRepository _hqRepository;

    public EdicoesController(IEdicaoRepository edicaoRepository, IHQRepository hqRepository)
    {
        _edicaoRepository = edicaoRepository;
        _hqRepository = hqRepository;
    }

    // GET: api/edicoes/hq/{hqId}
    [HttpGet("hq/{hqId:guid}")]
    public async Task<ActionResult<IEnumerable<EdicaoResponse>>> GetByHqId(Guid hqId)
    {
        // Verifica se a HQ existe
        var hq = await _hqRepository.GetByIdAsync(hqId);
        if (hq is null) return NotFound("HQ não encontrada.");

        var edicoes = await _edicaoRepository.GetByHqIdAsync(hqId);
        var response = edicoes.Select(e => new EdicaoResponse
        {
            Id = e.Id,
            HqId = e.HqId,
            Titulo = e.Titulo,
            Numero = e.Numero,
            Sinopse = e.Sinopse,
            Capa = e.Capa,
            Lida = e.Lida,
            Obs = e.Obs,
            Nota = e.Nota,
            DataLeitura = e.DataLeitura
        });
        return Ok(response);
    }

    // GET api/edicoes/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<EdicaoResponse>> GetById(Guid id)
    {
        var edicao = await _edicaoRepository.GetByIdAsync(id);
        if (edicao is null) return NotFound();

        var response = new EdicaoResponse
        {
            Id = edicao.Id,
            HqId = edicao.HqId,
            Titulo = edicao.Titulo,
            Numero = edicao.Numero,
            Sinopse = edicao.Sinopse,
            Capa = edicao.Capa,
            Lida = edicao.Lida,
            Obs = edicao.Obs,
            Nota = edicao.Nota,
            DataLeitura = edicao.DataLeitura
        };
        return Ok(response);
    }

    // POST api/edicoes
    [HttpPost]
    public async Task<ActionResult<EdicaoResponse>> Create([FromBody] EdicaoCreateRequest request)
    {
        // Verifica se a HQ existe
        var hq = await _hqRepository.GetByIdAsync(request.HqId);
        if (hq is null) return NotFound("HQ não encontrada.");

        // Verifica se já existe uma edição com o mesmo número para esta HQ
        var edicaoExistente = await _edicaoRepository.GetByHqIdAndNumeroAsync(request.HqId, request.Numero);
        if (edicaoExistente != null)
            return BadRequest("Já existe uma edição com este número para esta HQ.");

        var novaEdicao = new Domain.Entities.Edicao
        {
            HqId = request.HqId,
            Titulo = request.Titulo,
            Numero = request.Numero,
            Sinopse = request.Sinopse,
            Capa = request.Capa,
            Lida = request.Lida,
            Obs = request.Obs,
            Nota = request.Nota,
            DataLeitura = request.Lida ? request.DataLeitura ?? DateTime.UtcNow : null
        };

        await _edicaoRepository.AddAsync(novaEdicao);

        var response = new EdicaoResponse
        {
            Id = novaEdicao.Id,
            HqId = novaEdicao.HqId,
            Titulo = novaEdicao.Titulo,
            Numero = novaEdicao.Numero,
            Sinopse = novaEdicao.Sinopse,
            Capa = novaEdicao.Capa,
            Lida = novaEdicao.Lida,
            Obs = novaEdicao.Obs,
            Nota = novaEdicao.Nota,
            DataLeitura = novaEdicao.DataLeitura
        };

        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    // PUT api/edicoes/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] EdicaoUpdateRequest request)
    {
        var edicaoExistente = await _edicaoRepository.GetByIdAsync(id);
        if (edicaoExistente is null) return NotFound();

        // Verifica se já existe outra edição com o mesmo número para esta HQ
        if (edicaoExistente.Numero != request.Numero)
        {
            var edicaoComMesmoNumero = await _edicaoRepository.GetByHqIdAndNumeroAsync(edicaoExistente.HqId, request.Numero);
            if (edicaoComMesmoNumero != null && edicaoComMesmoNumero.Id != id)
                return BadRequest("Já existe outra edição com este número para esta HQ.");
        }

        edicaoExistente.Titulo = request.Titulo;
        edicaoExistente.Numero = request.Numero;
        edicaoExistente.Sinopse = request.Sinopse;
        edicaoExistente.Capa = request.Capa;
        edicaoExistente.Lida = request.Lida;
        edicaoExistente.Obs = request.Obs;
        edicaoExistente.Nota = request.Nota;

        // Atualiza a data de leitura apenas se foi marcada como lida e não tinha data antes
        if (request.Lida && !edicaoExistente.Lida)
        {
            edicaoExistente.DataLeitura = request.DataLeitura ?? DateTime.UtcNow;
        }
        else if (!request.Lida)
        {
            edicaoExistente.DataLeitura = null;
        }
        else if (request.Lida && request.DataLeitura.HasValue)
        {
            edicaoExistente.DataLeitura = request.DataLeitura;
        }

        await _edicaoRepository.UpdateAsync(edicaoExistente);

        return NoContent();
    }

    // DELETE api/edicoes/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var edicao = await _edicaoRepository.GetByIdAsync(id);
        if (edicao is null) return NotFound();

        await _edicaoRepository.DeleteAsync(id);
        return NoContent();
    }

    // PATCH api/edicoes/{id}/marcar-lida
    [HttpPatch("{id:guid}/marcar-lida")]
    public async Task<IActionResult> MarcarComoLida(Guid id, [FromBody] bool lida, [FromQuery] DateTime? dataLeitura = null)
    {
        var edicao = await _edicaoRepository.GetByIdAsync(id);
        if (edicao is null) return NotFound();

        edicao.Lida = lida;
        edicao.DataLeitura = lida ? dataLeitura ?? DateTime.UtcNow : null;

        await _edicaoRepository.UpdateAsync(edicao);

        return NoContent();
    }

    // GET api/edicoes/hq/{hqId}/estatisticas
    [HttpGet("hq/{hqId:guid}/estatisticas")]
    public async Task<ActionResult<object>> GetEstatisticasHq(Guid hqId)
    {
        var hq = await _hqRepository.GetByIdAsync(hqId);
        if (hq is null) return NotFound("HQ não encontrada.");

        var edicoes = await _edicaoRepository.GetByHqIdAsync(hqId);
        var totalEdicoes = edicoes.Count();
        var edicoesLidas = edicoes.Count(e => e.Lida);
        var porcentagemConcluida = totalEdicoes > 0 ? (edicoesLidas * 100) / totalEdicoes : 0;
        var mediaNotas = edicoes.Where(e => e.Nota.HasValue).Average(e => e.Nota);

        return Ok(new
        {
            TotalEdicoes = totalEdicoes,
            EdicoesLidas = edicoesLidas,
            PorcentagemConcluida = porcentagemConcluida,
            MediaNotas = mediaNotas,
            ProximaEdicao = edicoes.Where(e => !e.Lida).OrderBy(e => e.Numero).FirstOrDefault()?.Numero
        });
    }
}