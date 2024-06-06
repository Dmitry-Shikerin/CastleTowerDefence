using System;
using Sources.Frameworks.Domain.Interfaces.Data;
using Sources.Frameworks.Domain.Interfaces.Entities;

namespace Sources.InfrastructureInterfaces.Services.LoadServices.Collectors
{
    public interface IMapperCollector
    {
        Func<IEntity, IDto> GetToDtoMapper(Type type);
        Func<IDto, IEntity> GetToModelMapper(Type type);
    }
}