using Microsoft.Extensions.Options;
using MongoDB.Driver;
using HQManager.Domain.Entities;
using MongoDB.Bson;
using HQManager.Infra.Data.Config;
using HQManager.Infra.Data.Mappings;
using HQManager.Infra.Data.Config;

namespace HQManager.Infra.Data.Data;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IMongoDbSettings settings) 
    {
        var client = new MongoClient(settings.ConnectionString); 
        _database = client.GetDatabase(settings.DatabaseName);
        MapEntities();
    }

    // Propriedades para acessar as coleções
    public IMongoCollection<Usuario> Usuarios => _database.GetCollection<Usuario>("Usuarios");
    public IMongoCollection<Editora> Editoras => _database.GetCollection<Editora>("Editoras");
    public IMongoCollection<Personagem> Personagens => _database.GetCollection<Personagem>("Personagens");
    public IMongoCollection<Equipe> Equipes => _database.GetCollection<Equipe>("Equipes");
    public IMongoCollection<HQ> HQs => _database.GetCollection<HQ>("HQs");
    public IMongoCollection<Edicao> Edicoes => _database.GetCollection<Edicao>("Edicoes");

    // Método para configurar os mapeamentos
    private void MapEntities()
    {
        // Este método será chamado assim que o DbContext for instanciado.
        // Ele garante que nossos registros de classe estejam aplicados.
        EntityMap.RegisterClassMaps();
    }

    // Método utilitário para verificar se o MongoDB está conectado
    public async Task<bool> IsConnected(CancellationToken cancellationToken = default)
    {
        try
        {
            // Um comando simples e leve para testar a conexão
            await _database.RunCommandAsync((Command<BsonDocument>)"{ ping: 1 }", cancellationToken: cancellationToken);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}