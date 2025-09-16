using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HQManager.Application.DTOs.Auth;
using HQManager.Application.DTOs.Usuario;
using HQManager.CrossCutting.Config;
using HQManager.Domain.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HQManager.CrossCutting.Services;

public class JwtAuthService : IAuthService
{
    private readonly JwtSettings _jwtSettings;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IHashService _hashService;

    public JwtAuthService(IOptions<JwtSettings> jwtSettings, IUsuarioRepository usuarioRepository, IHashService hashService)
    {
        _jwtSettings = jwtSettings.Value;
        _usuarioRepository = usuarioRepository;
        _hashService = hashService;
    }

    public async Task<LoginResponse> LoginAsync(string email, string senha)
    {
        // 1. Buscar usuário pelo email
        var usuario = await _usuarioRepository.GetByEmailAsync(email);
        if (usuario is null)
            return LoginResponse.Falha("Usuário ou senha inválidos.");

        // 2. Verificar a senha
        if (!_hashService.VerifyPassword(senha, usuario.SenhaHash))
            return LoginResponse.Falha("Usuário ou senha inválidos.");

        // 3. Gerar o token JWT
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email)
            }),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        // 4. Atualizar data do último login
        usuario.UltimoLogin = DateTime.UtcNow;
        await _usuarioRepository.UpdateAsync(usuario);

        // 5. Mapear usuário para response
        var usuarioResponse = new UsuarioResponse
        {
            Id = usuario.Id,
            Email = usuario.Email,
            CriadoEm = usuario.CriadoEm,
            UltimoLogin = usuario.UltimoLogin
        };

        // 6. Retornar resposta de sucesso
        return LoginResponse.CriarSucesso(tokenString, tokenDescriptor.Expires.Value, usuarioResponse);
    }
}