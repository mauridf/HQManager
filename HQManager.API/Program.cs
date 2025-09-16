using HQManager.Infra.Data.Data;
using HQManager.Infra.Data.Config;
using HQManager.Infra.Data.Seed;
using Microsoft.Extensions.Options;
using HQManager.Domain.Interfaces;
using HQManager.Infra.Data.Repositories;
using HQManager.CrossCutting.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using HQManager.CrossCutting.Config; // Para JwtSettings
using HQManager.CrossCutting.Services; // Para IAuthService
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Adicionar servi�os ao container.
builder.Services.AddControllers(); // Habilita os Controladores

// 2. Configura e registra as MongoDbSettings (l� do appsettings.json)
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection(nameof(MongoDbSettings)));
builder.Services.AddSingleton<IMongoDbSettings>(sp =>
    sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);

// 3. Registra o MongoDbContext como um servi�o Singleton
builder.Services.AddSingleton<MongoDbContext>();

// 4. Registra os Repositories (Padr�o: Scoped)
builder.Services.AddScoped<IUsuarioRepository>(sp =>
{
    var dbContext = sp.GetRequiredService<MongoDbContext>();
    return new UsuarioRepository(dbContext.Usuarios);
});

builder.Services.AddScoped<IEditoraRepository>(sp =>
{
    var dbContext = sp.GetRequiredService<MongoDbContext>();
    return new EditoraRepository(dbContext.Editoras);
});

// 5. Registra o servi�o de Seed (Cria��o de �ndices) como HostedService
builder.Services.AddHostedService<MongoDbSeed>();

// 6. Configura e registra JwtSettings
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection(nameof(JwtSettings)));

// 7. Registra o servi�o de Autentica��o
builder.Services.AddScoped<IAuthService, JwtAuthService>();

// 8. Configura a Autentica��o JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
var key = Encoding.ASCII.GetBytes(jwtSettings.SecretKey);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false; // true em produ��o!
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtSettings.Audience,
        ValidateLifetime = true
    };
});

// 9. Configura a Autoriza��o
builder.Services.AddAuthorization();

// 10. Adiciona o Swagger para documentar e testar a API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "HQManager API", Version = "v1" });

    // Configura��o para suportar JWT no Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// 11. Registra o servi�o de Hash (Singleton)
builder.Services.AddSingleton<IHashService, BCryptHashService>();

var app = builder.Build();

// 12. Configura o pipeline de requisi��o HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers(); // Mapeia os endpoints dos controladores

// 13. (OPCIONAL) Cria um endpoint m�nimo para testar a conex�o com o BD
app.MapGet("/api/health", async (MongoDbContext context) =>
{
    var isConnected = await context.IsConnected();
    return isConnected
        ? Results.Ok("HQManager API est� online e conectada ao MongoDB!")
        : Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
});

app.Run();