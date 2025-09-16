using HQManager.Application.DTOs.Editora;
using HQManager.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HQManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Todos os endpoints exigem autenticação
public class EditorasController : ControllerBase
{
    private readonly IEditoraRepository _editoraRepository;

    public EditorasController(IEditoraRepository editoraRepository)
    {
        _editoraRepository = editoraRepository;
    }

    // GET: api/editoras
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EditoraResponse>>> GetAll()
    {
        var editoras = await _editoraRepository.GetAllAsync();
        var response = editoras.Select(e => new EditoraResponse
        {
            Id = e.Id,
            Nome = e.Nome,
            AnoCriacao = e.AnoCriacao,
            Logotipo = e.Logotipo,
            PaisOrigem = e.PaisOrigem,
            SiteOficial = e.SiteOficial
        });
        return Ok(response);
    }

    // GET api/editoras/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<EditoraResponse>> GetById(Guid id)
    {
        var editora = await _editoraRepository.GetByIdAsync(id);
        if (editora is null) return NotFound();

        var response = new EditoraResponse
        {
            Id = editora.Id,
            Nome = editora.Nome,
            AnoCriacao = editora.AnoCriacao,
            Logotipo = editora.Logotipo,
            PaisOrigem = editora.PaisOrigem,
            SiteOficial = editora.SiteOficial
        };
        return Ok(response);
    }

    // POST api/editoras
    [HttpPost]
    public async Task<ActionResult<EditoraResponse>> Create([FromBody] EditoraCreateRequest request)
    {
        var novaEditora = new Domain.Entities.Editora
        {
            Nome = request.Nome,
            AnoCriacao = request.AnoCriacao,
            Logotipo = request.Logotipo,
            PaisOrigem = request.PaisOrigem,
            SiteOficial = request.SiteOficial
        };

        await _editoraRepository.AddAsync(novaEditora);

        var response = new EditoraResponse
        {
            Id = novaEditora.Id,
            Nome = novaEditora.Nome,
            AnoCriacao = novaEditora.AnoCriacao,
            Logotipo = novaEditora.Logotipo,
            PaisOrigem = novaEditora.PaisOrigem,
            SiteOficial = novaEditora.SiteOficial
        };

        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    // PUT api/editoras/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] EditoraUpdateRequest request)
    {
        var editoraExistente = await _editoraRepository.GetByIdAsync(id);
        if (editoraExistente is null) return NotFound();

        editoraExistente.Nome = request.Nome;
        editoraExistente.AnoCriacao = request.AnoCriacao;
        editoraExistente.Logotipo = request.Logotipo;
        editoraExistente.PaisOrigem = request.PaisOrigem;
        editoraExistente.SiteOficial = request.SiteOficial;

        await _editoraRepository.UpdateAsync(editoraExistente);

        return NoContent(); // Retorna 204 No Content
    }

    // DELETE api/editoras/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var editora = await _editoraRepository.GetByIdAsync(id);
        if (editora is null) return NotFound();

        await _editoraRepository.DeleteAsync(id);
        return NoContent(); // Retorna 204 No Content
    }

    // GET api/editoras/nome/{nome}
    [HttpGet("nome/{nome}")]
    public async Task<ActionResult<EditoraResponse>> GetByNome(string nome)
    {
        var editora = await _editoraRepository.GetByNomeAsync(nome);
        if (editora is null) return NotFound();

        var response = new EditoraResponse
        {
            Id = editora.Id,
            Nome = editora.Nome,
            AnoCriacao = editora.AnoCriacao,
            Logotipo = editora.Logotipo,
            PaisOrigem = editora.PaisOrigem,
            SiteOficial = editora.SiteOficial
        };
        return Ok(response);
    }
}