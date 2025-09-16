using HQManager.Application.DTOs.Equipe;
using HQManager.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HQManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EquipesController : ControllerBase
{
    private readonly IEquipeRepository _equipeRepository;

    public EquipesController(IEquipeRepository equipeRepository)
    {
        _equipeRepository = equipeRepository;
    }

    // GET: api/equipes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EquipeResponse>>> GetAll()
    {
        var equipes = await _equipeRepository.GetAllAsync();
        var response = equipes.Select(e => new EquipeResponse
        {
            Id = e.Id,
            Nome = e.Nome,
            Descricao = e.Descricao,
            Imagem = e.Imagem,
            AnoCriacao = e.AnoCriacao
        });
        return Ok(response);
    }

    // GET api/equipes/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<EquipeResponse>> GetById(Guid id)
    {
        var equipe = await _equipeRepository.GetByIdAsync(id);
        if (equipe is null) return NotFound();

        var response = new EquipeResponse
        {
            Id = equipe.Id,
            Nome = equipe.Nome,
            Descricao = equipe.Descricao,
            Imagem = equipe.Imagem,
            AnoCriacao = equipe.AnoCriacao
        };
        return Ok(response);
    }

    // POST api/equipes
    [HttpPost]
    public async Task<ActionResult<EquipeResponse>> Create([FromBody] EquipeCreateRequest request)
    {
        var novaEquipe = new Domain.Entities.Equipe
        {
            Nome = request.Nome,
            Descricao = request.Descricao,
            Imagem = request.Imagem,
            AnoCriacao = request.AnoCriacao
        };

        await _equipeRepository.AddAsync(novaEquipe);

        var response = new EquipeResponse
        {
            Id = novaEquipe.Id,
            Nome = novaEquipe.Nome,
            Descricao = novaEquipe.Descricao,
            Imagem = novaEquipe.Imagem,
            AnoCriacao = novaEquipe.AnoCriacao
        };

        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    // PUT api/equipes/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] EquipeUpdateRequest request)
    {
        var equipeExistente = await _equipeRepository.GetByIdAsync(id);
        if (equipeExistente is null) return NotFound();

        equipeExistente.Nome = request.Nome;
        equipeExistente.Descricao = request.Descricao;
        equipeExistente.Imagem = request.Imagem;
        equipeExistente.AnoCriacao = request.AnoCriacao;

        await _equipeRepository.UpdateAsync(equipeExistente);

        return NoContent();
    }

    // DELETE api/equipes/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var equipe = await _equipeRepository.GetByIdAsync(id);
        if (equipe is null) return NotFound();

        await _equipeRepository.DeleteAsync(id);
        return NoContent();
    }
}