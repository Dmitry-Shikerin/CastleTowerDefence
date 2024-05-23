using System;

namespace Sources.DomainInterfaces.Entities
{
    public interface IEntity
    {
        string Id { get; }
        Type Type { get; }
    }
}