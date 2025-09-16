using HQManager.Domain.Interfaces;

namespace HQManager.Domain.Entities;

public abstract class EntityBase : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
}