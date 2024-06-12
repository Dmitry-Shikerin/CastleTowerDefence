using System;
using Sources.Frameworks.Domain.Interfaces.Data;
using Sources.Frameworks.Domain.Interfaces.Entities;

namespace Sources.Frameworks.GameServices.Loads.Services.Interfaces.Data
{
    public interface IDataService
    {
        object LoadData(string key, Type dtoType);
        T LoadData<T>(string key) 
            where T : IEntity;
        void SaveData<T>(T dataModel, string key) 
            where T : IEntity;
        bool HasKey(string key);
        void Clear(string key);
    }
}