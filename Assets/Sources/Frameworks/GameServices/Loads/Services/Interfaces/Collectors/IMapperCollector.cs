using System;
using Sources.Frameworks.Domain.Interfaces.Data;
using Sources.Frameworks.Domain.Interfaces.Entities;

namespace Sources.Frameworks.GameServices.Loads.Services.Interfaces.Collectors
{
    public interface IMapperCollector
    {
        Func<IEntity, IDto> GetToDtoMapper(Type type);
        Func<IDto, IEntity> GetToModelMapper(Type type);
    }
}