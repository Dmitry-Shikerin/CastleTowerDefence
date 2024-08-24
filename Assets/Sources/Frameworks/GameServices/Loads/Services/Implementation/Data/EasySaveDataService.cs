using System;
using Sources.Frameworks.Domain.Interfaces.Entities;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces.Data;

namespace Sources.Frameworks.GameServices.Loads.Services.Implementation.Data
{
    public class EasySaveDataService : IDataService
    {
        public object LoadData(string key, Type type) =>
            ES3.Load(key, new object());

        public T LoadData<T>(string key) where T : IEntity =>
            ES3.Load<T>(key);

        public void SaveData<T>(T dataModel, string key) where T : IEntity =>
            ES3.Save(key, dataModel);

        public bool HasKey(string key) =>
            ES3.KeyExists(key);

        public void Clear(string key) =>
            ES3.DeleteKey(key);
    }
}