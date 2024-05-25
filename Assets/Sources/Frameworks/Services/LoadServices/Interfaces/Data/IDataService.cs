using System;
using Sources.DomainInterfaces.Models.Data;

namespace Sources.InfrastructureInterfaces.Services.LoadServices.Data
{
    public interface IDataService
    {
        object LoadData(string key, Type dtoType);
        T LoadData<T>(string key) 
            where T : IDto;
        void SaveData<T>(T dataModel, string key) 
            where T : IDto;
        bool HasKey(string key);
        void Clear(string key);
    }
}