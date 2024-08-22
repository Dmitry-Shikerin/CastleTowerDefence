using System.Collections.Generic;
using Sources.Frameworks.Domain.Interfaces.Entities;

namespace Sources.Frameworks.GameServices.Repositories.Services.Interfaces
{
    public interface IEntityRepository
    {
        IReadOnlyDictionary<string, IEntity> Entities { get; }
        
        void Add(IEntity entity);
        IEntity Get(string id);
        T Get<T>(string id) 
            where T : class, IEntity;
        void Release();
        IEnumerable<T> GetAll<T>(IEnumerable<string> ids) 
            where T : class, IEntity;
    }
}