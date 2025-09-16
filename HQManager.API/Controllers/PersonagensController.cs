using HQManager.Application.DTOs.Personagem;
using HQManager.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HQManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PersonagensController : ControllerBase
{
    private readonly IPersonagemRepository _personagemRepository;

    public PersonagensController(IPersonagemRepository personagemRepository)
    {
        _personagemRepository = personagemRepository;
    }

    // GET: api/personagens
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PersonagemResponse>>> GetAll()
    {
        var personagens = await _personagemRepository.GetAllAsync();
        var response = personagens.Select(p => new PersonagemResponse
        {
            Id = p.Id,
            Nome = p.Nome,
            Tipo = p.Tipo,
            Descricao = p.Descricao,
            Imagem = p.Imagem,
            PrimeiraAparicao = p.PrimeiraAparicao
        });
        return Ok(response);
    }

    // GET api/personagens/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PersonagemResponse>> GetById(Guid id)
    {
        var personagem = await _personagemRepository.GetByIdAsync(id);
        if (personagem is null) return NotFound();

        var response = new PersonagemResponse
        {
            Id = personagem.Id,
            Nome = personagem.Nome,
            Tipo = personagem.Tipo,
            Descricao = personagem.Descricao,
            Imagem = personagem.Imagem,
            PrimeiraAparicao = personagem.PrimeiraAparicao
        };
        return Ok(response);
    }

    // POST api/personagens
    [HttpPost]
    public async Task<ActionResult<PersonagemResponse>> Create([FromBody] PersonagemCreateRequest request)
    {
        var novoPersonagem = new Domain.Entities.Personagem
        {
            Nome = request.Nome,
            Tipo = request.Tipo,
            Descricao = request.Descricao,
            Imagem = request.Imagem,
            PrimeiraAparicao = request.PrimeiraAparicao
        };

        await _personagemRepository.AddAsync(novoPersonagem);

        var response = new PersonagemResponse
        {
            Id = novoPersonagem.Id,
            Nome = novoPersonagem.Nome,
            Tipo = novoPersonagem.Tipo,
            Descricao = novoPersonagem.Descricao,
            Imagem = novoPersonagem.Imagem,
            PrimeiraAparicao = novoPersonagem.PrimeiraAparicao
        };

        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    // PUT api/personagens/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] PersonagemUpdateRequest request)
    {
        var personagemExistente = await _personagemRepository.GetByIdAsync(id);
        if (personagemExistente is null) return NotFound();

        personagemExistente.Nome = request.Nome;
        personagemExistente.Tipo = request.Tipo;
        personagemExistente.Descricao = request.Descricao;
        personagemExistente.Imagem = request.Imagem;
        personagemExistente.PrimeiraAparicao = request.PrimeiraAparicao;

        await _personagemRepository.UpdateAsync(personagemExistente);

        return NoContent();
    }

    // DELETE api/personagens/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var personagem = await _personagemRepository.GetByIdAsync(id);
        if (personagem is null) return NotFound();

        await _personagemRepository.DeleteAsync(id);
        return NoContent();
    }

    // GET api/personagens/tipos
    [HttpGet("tipos")]
    [AllowAnonymous] // Pode ser acessado sem autenticação
    public ActionResult<IEnumerable<object>> GetTiposPersonagem()
    {
        var tipos = Enum.GetValues(typeof(Domain.Enums.TipoPersonagem))
            .Cast<Domain.Enums.TipoPersonagem>()
            .Select(t => new
            {
                Valor = (int)t,
                Nome = t.ToString(),
                Descricao = ObterDescricaoTipo(t)
            })
            .ToList();

        return Ok(tipos);
    }

    private string ObterDescricaoTipo(Domain.Enums.TipoPersonagem tipo)
    {
        return tipo switch
        {
            Domain.Enums.TipoPersonagem.Heroi => "Herói",
            Domain.Enums.TipoPersonagem.Vilao => "Vilão",
            Domain.Enums.TipoPersonagem.AntiHeroi => "Anti-herói",
            Domain.Enums.TipoPersonagem.Fantasia => "Personagem de Fantasia",
            Domain.Enums.TipoPersonagem.Historico => "Personagem Histórico",
            Domain.Enums.TipoPersonagem.Real => "Personagem Real",
            Domain.Enums.TipoPersonagem.Fabula => "Personagem de Fábula",
            Domain.Enums.TipoPersonagem.Outro => "Outro tipo de personagem",
            _ => "Tipo desconhecido"
        };
    }
}