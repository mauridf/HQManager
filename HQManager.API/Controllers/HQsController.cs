using HQManager.Application.DTOs.HQ;
using HQManager.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HQManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class HQsController : ControllerBase
{
    private readonly IHQRepository _hqRepository;
    private readonly IPersonagemRepository _personagemRepository;
    private readonly IEquipeRepository _equipeRepository;
    private readonly IEditoraRepository _editoraRepository;

    public HQsController(
        IHQRepository hqRepository,
        IPersonagemRepository personagemRepository,
        IEquipeRepository equipeRepository,
        IEditoraRepository editoraRepository)
    {
        _hqRepository = hqRepository;
        _personagemRepository = personagemRepository;
        _equipeRepository = equipeRepository;
        _editoraRepository = editoraRepository;
    }

    // GET: api/hqs
    [HttpGet]
    public async Task<ActionResult<IEnumerable<HQResponse>>> GetAll()
    {
        var hqs = await _hqRepository.GetAllAsync();
        var response = hqs.Select(hq => new HQResponse
        {
            Id = hq.Id,
            Nome = hq.Nome,
            NomeOriginal = hq.NomeOriginal,
            TipoPublicacao = hq.TipoPublicacao,
            Status = hq.Status,
            TotalEdicoes = hq.TotalEdicoes,
            Sinopse = hq.Sinopse,
            AnoLancamento = hq.AnoLancamento,
            LeiturasRecomendadas = hq.LeiturasRecomendadas,
            Imagem = hq.Imagem,
            Tags = hq.Tags,
            CriadoEm = hq.CriadoEm,
            Personagens = hq.Personagens,
            Equipes = hq.Equipes,
            Editoras = hq.Editoras
        });
        return Ok(response);
    }

    // GET api/hqs/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<HQResponse>> GetById(Guid id)
    {
        var hq = await _hqRepository.GetByIdAsync(id);
        if (hq is null) return NotFound();

        var response = new HQResponse
        {
            Id = hq.Id,
            Nome = hq.Nome,
            NomeOriginal = hq.NomeOriginal,
            TipoPublicacao = hq.TipoPublicacao,
            Status = hq.Status,
            TotalEdicoes = hq.TotalEdicoes,
            Sinopse = hq.Sinopse,
            AnoLancamento = hq.AnoLancamento,
            LeiturasRecomendadas = hq.LeiturasRecomendadas,
            Imagem = hq.Imagem,
            Tags = hq.Tags,
            CriadoEm = hq.CriadoEm,
            Personagens = hq.Personagens,
            Equipes = hq.Equipes,
            Editoras = hq.Editoras
        };
        return Ok(response);
    }

    // POST api/hqs
    [HttpPost]
    public async Task<ActionResult<HQResponse>> Create([FromBody] HQCreateRequest request)
    {
        // VALIDAÇÃO DA REGRA DE NEGÓCIO: HQ deve ter pelo menos um personagem OU uma equipe
        if ((request.Personagens == null || !request.Personagens.Any()) &&
            (request.Equipes == null || !request.Equipes.Any()))
        {
            return BadRequest("Uma HQ deve estar relacionada a pelo menos um Personagem ou uma Equipe.");
        }

        // Valida se os IDs de relacionamento existem (opcional, mas recomendado)
        // await ValidarRelacionamentosExistem(request);

        var novaHQ = new Domain.Entities.HQ
        {
            Nome = request.Nome,
            NomeOriginal = request.NomeOriginal,
            TipoPublicacao = request.TipoPublicacao,
            Status = request.Status,
            TotalEdicoes = request.TotalEdicoes,
            Sinopse = request.Sinopse,
            AnoLancamento = request.AnoLancamento,
            LeiturasRecomendadas = request.LeiturasRecomendadas ?? new(),
            Imagem = request.Imagem,
            Tags = request.Tags,
            CriadoEm = DateTime.UtcNow,
            Personagens = request.Personagens,
            Equipes = request.Equipes,
            Editoras = request.Editoras
        };

        await _hqRepository.AddAsync(novaHQ);

        var response = new HQResponse
        {
            Id = novaHQ.Id,
            Nome = novaHQ.Nome,
            NomeOriginal = novaHQ.NomeOriginal,
            TipoPublicacao = novaHQ.TipoPublicacao,
            Status = novaHQ.Status,
            TotalEdicoes = novaHQ.TotalEdicoes,
            Sinopse = novaHQ.Sinopse,
            AnoLancamento = novaHQ.AnoLancamento,
            LeiturasRecomendadas = novaHQ.LeiturasRecomendadas,
            Imagem = novaHQ.Imagem,
            Tags = novaHQ.Tags,
            CriadoEm = novaHQ.CriadoEm,
            Personagens = novaHQ.Personagens,
            Equipes = novaHQ.Equipes,
            Editoras = novaHQ.Editoras
        };

        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    // PUT api/hqs/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] HQUpdateRequest request)
    {
        // VALIDAÇÃO DA REGRA DE NEGÓCIO
        if ((request.Personagens == null || !request.Personagens.Any()) &&
            (request.Equipes == null || !request.Equipes.Any()))
        {
            return BadRequest("Uma HQ deve estar relacionada a pelo menos um Personagem ou uma Equipe.");
        }

        var hqExistente = await _hqRepository.GetByIdAsync(id);
        if (hqExistente is null) return NotFound();

        // Valida se os IDs de relacionamento existem
        // await ValidarRelacionamentosExistem(request);

        hqExistente.Nome = request.Nome;
        hqExistente.NomeOriginal = request.NomeOriginal;
        hqExistente.TipoPublicacao = request.TipoPublicacao;
        hqExistente.Status = request.Status;
        hqExistente.TotalEdicoes = request.TotalEdicoes;
        hqExistente.Sinopse = request.Sinopse;
        hqExistente.AnoLancamento = request.AnoLancamento;
        hqExistente.LeiturasRecomendadas = request.LeiturasRecomendadas ?? new();
        hqExistente.Imagem = request.Imagem;
        hqExistente.Tags = request.Tags;
        hqExistente.Personagens = request.Personagens;
        hqExistente.Equipes = request.Equipes;
        hqExistente.Editoras = request.Editoras;

        await _hqRepository.UpdateAsync(hqExistente);

        return NoContent();
    }

    // DELETE api/hqs/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var hq = await _hqRepository.GetByIdAsync(id);
        if (hq is null) return NotFound();

        await _hqRepository.DeleteAsync(id);
        return NoContent();
    }

    // Método auxiliar para validar relacionamentos (opcional)
    private async Task<bool> ValidarRelacionamentosExistem(HQCreateRequest request)
    {
        // Validação de Personagens
        foreach (var personagemId in request.Personagens)
        {
            var personagem = await _personagemRepository.GetByIdAsync(personagemId);
            if (personagem is null)
                throw new ArgumentException($"Personagem com ID {personagemId} não encontrado.");
        }

        // Validação de Equipes
        foreach (var equipeId in request.Equipes)
        {
            var equipe = await _equipeRepository.GetByIdAsync(equipeId);
            if (equipe is null)
                throw new ArgumentException($"Equipe com ID {equipeId} não encontrado.");
        }

        // Validação de Editoras
        foreach (var editoraId in request.Editoras)
        {
            var editora = await _editoraRepository.GetByIdAsync(editoraId);
            if (editora is null)
                throw new ArgumentException($"Editora com ID {editoraId} não encontrado.");
        }

        return true;
    }

    // GET api/hqs/personagem/{personagemId}
    [HttpGet("personagem/{personagemId:guid}")]
    public async Task<ActionResult<IEnumerable<HQResponse>>> GetByPersonagemId(Guid personagemId)
    {
        var hqs = await _hqRepository.GetByPersonagemIdAsync(personagemId);
        var response = hqs.Select(hq => new HQResponse
        {
            Id = hq.Id,
            Nome = hq.Nome,
            // ... mapear outros campos conforme necessário ...
            Personagens = hq.Personagens
        });
        return Ok(response);
    }

    // GET api/hqs/equipe/{equipeId}
    [HttpGet("equipe/{equipeId:guid}")]
    public async Task<ActionResult<IEnumerable<HQResponse>>> GetByEquipeId(Guid equipeId)
    {
        var hqs = await _hqRepository.GetByEquipeIdAsync(equipeId);
        var response = hqs.Select(hq => new HQResponse
        {
            Id = hq.Id,
            Nome = hq.Nome,
            Equipes = hq.Equipes
        });
        return Ok(response);
    }

    // GET api/hqs/editora/{editoraId}
    [HttpGet("editora/{editoraId:guid}")]
    public async Task<ActionResult<IEnumerable<HQResponse>>> GetByEditoraId(Guid editoraId)
    {
        var hqs = await _hqRepository.GetByEditoraIdAsync(editoraId);
        var response = hqs.Select(hq => new HQResponse
        {
            Id = hq.Id,
            Nome = hq.Nome,
            Editoras = hq.Editoras
        });
        return Ok(response);
    }
}