using HQManager.Application.DTOs.Auth;
using HQManager.CrossCutting.Services;
using Microsoft.AspNetCore.Mvc;

namespace HQManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        var resultado = await _authService.LoginAsync(request.Email, request.Senha);

        if (!resultado.Sucesso)
            return Unauthorized(resultado); // Retorna 401 com a mensagem de falha

        return Ok(resultado); // Retorna 200 com o token e dados do usuário
    }
}