using Sources.DomainInterfaces.Entities;

namespace Sources.InfrastructureInterfaces.Services.LoadServices
{
    public interface ILoadService
    {
        T Load<T>(string id) 
            where T : class, IEntity;
        void Save(IEntity entity);
        void Save(string id);
        void LoadAll();
        void SaveAll();
        void Clear(IEntity entity);
        void Clear(string id);
        void ClearAll();
        bool HasKey(string id);
    }
}