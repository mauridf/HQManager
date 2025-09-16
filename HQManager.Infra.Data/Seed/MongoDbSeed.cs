using HQManager.Domain.Entities;
using HQManager.Infra.Data.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace HQManager.Infra.Data.Seed;

public class MongoDbSeed : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MongoDbSeed> _logger;

    public MongoDbSeed(IServiceProvider serviceProvider, ILogger<MongoDbSeed> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MongoDbContext>();

        // Cria índice único para o Email do Usuário
        var usuarioIndexKeys = Builders<Usuario>.IndexKeys.Ascending(u => u.Email);
        var usuarioIndexOptions = new CreateIndexOptions { Unique = true };
        var usuarioIndexModel = new CreateIndexModel<Usuario>(usuarioIndexKeys, usuarioIndexOptions);

        await dbContext.Usuarios.Indexes.CreateOneAsync(usuarioIndexModel, cancellationToken: cancellationToken);
        _logger.LogInformation("Índice único para 'Email' na coleção 'Usuarios' criado/verificado.");

        // Adicione aqui a criação de outros índices futuros...
        // await dbContext.HQs.Indexes.CreateOneAsync(...);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}