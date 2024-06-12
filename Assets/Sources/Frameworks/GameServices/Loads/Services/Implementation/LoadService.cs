using System;
using System.Collections.Generic;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.Frameworks.Domain.Interfaces.Entities;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces.Data;
using Sources.InfrastructureInterfaces.Services.Repositories;

namespace Sources.Frameworks.GameServices.Loads.Services.Implementation
{
    public class LoadService : ILoadService
    {
        private readonly IEntityRepository _entityRepository;
        private readonly IDataService _dataService;
        // private readonly IMapperCollector _mapperCollector;

        public LoadService(
            IEntityRepository entityRepository,
            IDataService dataService)
        {
            _entityRepository = entityRepository ?? throw new ArgumentNullException(nameof(entityRepository));
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
            // _mapperCollector = mapperCollector ?? throw new ArgumentNullException(nameof(mapperCollector));
        }
        
        //todo лучше не придумал
        public T Load<T>(string id) 
            where T : class, IEntity
        {
            object entity = _dataService.LoadData(id, typeof(IEntity));
            // Func<IDto, IEntity> modelMapper = _mapperCollector.GetToModelMapper(ModelId.DtoTypes[id]);
            // IEntity model = modelMapper.Invoke((IDto)dto);

            if (entity is not T concrete)
                throw new InvalidCastException(nameof(T));
            
            _entityRepository.Add(concrete);

            return concrete;
        }

        public void Save(IEntity entity)
        {
            // Func<IEntity, IDto> dtoMapper = _mapperCollector.GetToDtoMapper(entity.Type);
            // IDto dto = dtoMapper.Invoke(entity);
            _dataService.SaveData(entity, entity.Id);
        }

        public void Save(string id)
        {
            IEntity entity = _entityRepository.Get(id);
            // Func<IEntity, IDto> dtoMapper = _mapperCollector.GetToDtoMapper(entity.Type);
            // IDto dto = dtoMapper.Invoke(entity);
            _dataService.SaveData(entity, entity.Id);
        }

        public void Load(IEnumerable<string> ids)
        {
            foreach (string id in ids)
            {
                object entity = _dataService.LoadData(id, typeof(IEntity));
                _entityRepository.Add((IEntity)entity);
            }
        }

        public void LoadAll()
        {
            foreach (string id in ModelId.ModelsIds)
            {
                // Type dtoType = ModelId.DtoTypes[id];
                object entity = _dataService.LoadData(id, typeof(IEntity));
                // Func<IDto, IEntity> mapper = _mapperCollector.GetToModelMapper(dtoType);
                // IEntity model = mapper.Invoke((IDto)dto);
                _entityRepository.Add((IEntity)entity);
            }
        }

        public void SaveAll()
        {
            foreach (IEntity entity in _entityRepository.Entities.Values)
            {
                // Func<IEntity, IDto> dtoMapper = _mapperCollector.GetToDtoMapper(dataModel.Type);
                // IDto dto = dtoMapper.Invoke(dataModel);
                _dataService.SaveData(entity, entity.Id);
            }
        }

        public void Save(IEnumerable<string> ids)
        {
            foreach (string id in ids)
            {
                IEntity entity = _entityRepository.Get(id);
                _dataService.SaveData(entity, id);
            }
        }

        public void Clear(IEntity entity) =>
            _dataService.Clear(entity.Id);

        public void Clear(string id) =>
            _dataService.Clear(id);

        public void ClearAll()
        {
            foreach (string id in ModelId.DeletedModelsIds)
                _dataService.Clear(id);
        }

        public bool HasKey(string id) =>
            _dataService.HasKey(id);
    }
}