using System;

namespace Sources.Frameworks.Domain.Interfaces.Entities
{
    public interface IEntity
    {
        string Id { get; }
        Type Type { get; }
    }
}