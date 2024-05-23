using System;
using System.Collections.Generic;
using Sources.DomainInterfaces.Entities;
using Sources.InfrastructureInterfaces.Services.Repositories;
using UnityEngine;

namespace Sources.Infrastructure.Services.Repositories
{
    public class EntityRepository : IEntityRepository
    {
        private readonly Dictionary<string, IEntity> _entities = new Dictionary<string, IEntity>();

        public IReadOnlyDictionary<string, IEntity> Entities => _entities;
        
        public void Add(IEntity entity)
        {
            if (_entities.ContainsKey(entity.Id))
                throw new InvalidOperationException($"Entity {entity.Id} with this Id already exists");
            
            _entities[entity.Id] = entity;
        }

        public IEntity Get(string id)
        {
            if (_entities.ContainsKey(id) == false)
                throw new InvalidOperationException($"Entity {id} with this Id does not exist");

            return _entities[id];
        }

        public T Get<T>(string id) where T : class, IEntity
        {
            if (_entities.ContainsKey(id) == false) 
                throw new InvalidOperationException($"Entity {id} with this Id does not exist");

            if(_entities[id] is not T concreteEntity)
                throw new InvalidCastException($"Entity {id} with this Id does not exist");
            
            return concreteEntity;
        }

        public void Release() =>
            _entities.Clear();
    }
}