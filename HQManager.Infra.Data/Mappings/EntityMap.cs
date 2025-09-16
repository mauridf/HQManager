using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using HQManager.Domain.Entities;
using HQManager.Domain.Enums;

namespace HQManager.Infra.Data.Mappings;

public static class EntityMap
{
    public static void RegisterClassMaps()
    {
        // CONFIGURAÇÃO GLOBAL PARA GUID (Solução do erro)
        // Define que todos os Guids serão serializados como BsonType.String
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

        // Convention Pack para ignorar valores default (evita salvar null/0 no BD)
        var conventionPack = new ConventionPack
        {
            new IgnoreIfDefaultConvention(true),
            new IgnoreExtraElementsConvention(true) // Ignora campos no BD que não estão na classe
        };
        ConventionRegistry.Register("My Conventions", conventionPack, t => true);

        // Registra os mapeamentos individuais
        BsonClassMap.RegisterClassMap<Usuario>(cm =>
        {
            cm.AutoMap();
            cm.SetIgnoreExtraElements(true);
        });

        BsonClassMap.RegisterClassMap<Editora>(cm =>
        {
            cm.AutoMap();
            cm.SetIgnoreExtraElements(true);
        });

        BsonClassMap.RegisterClassMap<Personagem>(cm =>
        {
            cm.AutoMap();
            // Mapeia o Enum para ser salvo como String, não como número
            cm.MapMember(p => p.Tipo).SetSerializer(new EnumSerializer<TipoPersonagem>(BsonType.String));
            cm.SetIgnoreExtraElements(true);
        });

        BsonClassMap.RegisterClassMap<Equipe>(cm =>
        {
            cm.AutoMap();
            cm.SetIgnoreExtraElements(true);
        });

        BsonClassMap.RegisterClassMap<HQ>(cm =>
        {
            cm.AutoMap();
            // Mapeia os Enums para String
            cm.MapMember(h => h.TipoPublicacao).SetSerializer(new EnumSerializer<TipoPublicacao>(BsonType.String));
            cm.MapMember(h => h.Status).SetSerializer(new EnumSerializer<StatusHQ>(BsonType.String));
            cm.SetIgnoreExtraElements(true);
        });

        BsonClassMap.RegisterClassMap<Edicao>(cm =>
        {
            cm.AutoMap();
            cm.SetIgnoreExtraElements(true);
        });
    }
}