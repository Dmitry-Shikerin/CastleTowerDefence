using System;
using Newtonsoft.Json;
using Sources.Frameworks.Domain.Interfaces.Entities;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces.Data;
using UnityEngine;

namespace Sources.Frameworks.GameServices.Loads.Services.Implementation.Data
{
    public class PlayerPrefsDataService : IDataService
    {
        public T LoadData<T>(string key)
            where T : IEntity => 
            (T)LoadData(key, typeof(T));

        public object LoadData(string key, Type type)
        {
            string json = PlayerPrefs.GetString(key, string.Empty);

            if (string.IsNullOrEmpty(json))
                throw new NullReferenceException(nameof(key));

            return JsonConvert.DeserializeObject(json, type) ?? 
                   throw new NullReferenceException(nameof(json));
        }
        
        public void SaveData<T>(T dataModel, string key)
            where T : IEntity
        {
            string json = JsonConvert.SerializeObject(dataModel) ?? 
                          throw new NullReferenceException(nameof(dataModel));
            
            PlayerPrefs.SetString(key, json);
        }

        public bool HasKey(string key) =>
            PlayerPrefs.HasKey(key);

        public void Clear(string key) =>
            PlayerPrefs.DeleteKey(key);
    }
}