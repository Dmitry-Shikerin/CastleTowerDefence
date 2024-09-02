using System;
using System.Collections.Generic;
using Sources.Frameworks.Domain.Interfaces.Entities;
using Sources.Frameworks.GameServices.Loads.Domain.Constant;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces.Data;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;

namespace Sources.Frameworks.GameServices.Loads.Services.Implementation
{
    public class LoadService : ILoadService
    {
        private readonly IEntityRepository _entityRepository;
        private readonly IDataService _dataService;

        public LoadService(
            IEntityRepository entityRepository,
            IDataService dataService)
        {
            _entityRepository = entityRepository ?? throw new ArgumentNullException(nameof(entityRepository));
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
        }
        
        public T Load<T>(string id) 
            where T : class, IEntity
        {
            object entity = _dataService.LoadData(id, ModelId.ModelData[id].Type);

            if (entity == null)
                throw new NullReferenceException(id);
            
            if (entity is not T concrete)
                throw new InvalidCastException(nameof(T));

            if (concrete.Id == null)
                throw new NullReferenceException(typeof(T).Name);
            
            _entityRepository.Add(concrete);

            return concrete;
        }

        public void Save(IEntity entity) =>
            _dataService.SaveData(entity, entity.Id);

        public void Save(string id)
        {
            IEntity entity = _entityRepository.Get(id);
            _dataService.SaveData(entity, entity.Id);
        }

        public void Load(IEnumerable<string> ids)
        {
            foreach (string id in ids)
            {
                object entity = _dataService.LoadData(id, ModelId.ModelData[id].Type);
                _entityRepository.Add((IEntity)entity);
            }
        }

        public void LoadAll()
        {
            foreach (string id in ModelId.ModelData.Keys)
            {
                Type type = ModelId.ModelData[id].Type;
                object entity = _dataService.LoadData(id, type);
                _entityRepository.Add((IEntity)entity);
            }
        }

        public void SaveAll()
        {
            foreach (IEntity entity in _entityRepository.Entities.Values)
                _dataService.SaveData(entity, entity.Id);
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
            foreach (string id in ModelId.ModelData.Keys)
                _dataService.Clear(id);
        }

        public void Clear(IEnumerable<string> ids)
        {
            foreach (string id in ids)
                _dataService.Clear(id);
        }

        public bool HasKey(string id) =>
            _dataService.HasKey(id);
    }
}