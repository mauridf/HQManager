using HQManager.Infra.Data.Data;
using HQManager.Infra.Data.Config;
using HQManager.Infra.Data.Seed;
using Microsoft.Extensions.Options;
using HQManager.Domain.Interfaces;
using HQManager.Infra.Data.Repositories;
using HQManager.CrossCutting.Services;

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

// 5. Registra o servi�o de Seed (Cria��o de �ndices) como HostedService
builder.Services.AddHostedService<MongoDbSeed>();

// 6. Adiciona o Swagger para documentar e testar a API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 7. Registra o servi�o de Hash (Singleton)
builder.Services.AddSingleton<IHashService, BCryptHashService>();

var app = builder.Build();

// 8. Configura o pipeline de requisi��o HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers(); // Mapeia os endpoints dos controladores

// 9. (OPCIONAL) Cria um endpoint m�nimo para testar a conex�o com o BD
app.MapGet("/api/health", async (MongoDbContext context) =>
{
    var isConnected = await context.IsConnected();
    return isConnected
        ? Results.Ok("HQManager API est� online e conectada ao MongoDB!")
        : Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
});

app.Run();