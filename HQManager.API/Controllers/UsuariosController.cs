// HQManager.API/Controllers/UsuariosController.cs
using HQManager.Application.DTOs.Usuario;
using HQManager.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using HQManager.Domain.Entities;
using HQManager.CrossCutting.Services;
using Microsoft.AspNetCore.Authorization;

namespace HQManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IHashService _hashService;

    public UsuariosController(IUsuarioRepository usuarioRepository, IHashService hashService)
    {
        _usuarioRepository = usuarioRepository;
        _hashService = hashService;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<UsuarioResponse>> Post([FromBody] UsuarioCreateRequest request)
    {
        // 1. Validar se o email já está em uso
        if (await _usuarioRepository.EmailEmUsoAsync(request.Email))
            return BadRequest("Este email já está em uso.");

        // 2. Fazer o hash da senha
        var senhaHash = _hashService.HashPassword(request.Senha);

        // 3. Mapear Request -> Entidade
        var novoUsuario = new Usuario
        {
            Email = request.Email,
            SenhaHash = senhaHash,
            CriadoEm = DateTime.UtcNow // Usar UTC é uma boa prática
        };

        // 4. Salvar no banco
        await _usuarioRepository.AddAsync(novoUsuario);

        // 5. Mapear Entidade -> Response
        var response = new UsuarioResponse
        {
            Id = novoUsuario.Id,
            Email = novoUsuario.Email,
            CriadoEm = novoUsuario.CriadoEm,
            UltimoLogin = novoUsuario.UltimoLogin
        };

        // 6. Retornar 201 Created
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    // Método auxiliar para o CreatedAtAction
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UsuarioResponse>> GetById(Guid id)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(id);
        if (usuario is null) return NotFound();

        return new UsuarioResponse
        {
            Id = usuario.Id,
            Email = usuario.Email,
            CriadoEm = usuario.CriadoEm,
            UltimoLogin = usuario.UltimoLogin
        };
    }
}