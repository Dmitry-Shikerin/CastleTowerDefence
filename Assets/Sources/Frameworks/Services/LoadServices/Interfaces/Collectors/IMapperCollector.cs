using System;
using Sources.DomainInterfaces.Entities;
using Sources.DomainInterfaces.Models.Data;

namespace Sources.InfrastructureInterfaces.Services.LoadServices.Collectors
{
    public interface IMapperCollector
    {
        Func<IEntity, IDto> GetToDtoMapper(Type type);
        Func<IDto, IEntity> GetToModelMapper(Type type);
    }
}