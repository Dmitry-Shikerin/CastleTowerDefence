using System.Collections.Generic;
using Sources.Frameworks.Domain.Interfaces.Entities;

namespace Sources.Frameworks.GameServices.Loads.Services.Interfaces
{
    public interface ILoadService
    {
        T Load<T>(string id) 
            where T : class, IEntity;
        void Save(IEntity entity);
        void Save(string id);
        void SaveAll();
        void Save(IEnumerable<string> ids);
        void Load(IEnumerable<string> ids);
        void LoadAll();
        void Clear(IEntity entity);
        void Clear(string id);
        void ClearAll();
        void Clear(IEnumerable<string> ids);
        bool HasKey(string id);
    }
}