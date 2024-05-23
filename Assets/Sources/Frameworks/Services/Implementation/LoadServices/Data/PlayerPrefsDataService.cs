using System;
using Newtonsoft.Json;
using Sources.DomainInterfaces.Models.Data;
using Sources.InfrastructureInterfaces.Services.LoadServices.Data;
using UnityEngine;

namespace Sources.Infrastructure.Services.LoadServices.Data
{
    public class PlayerPrefsDataService : IDataService
    {
        public T LoadData<T>(string key)
            where T : IDto => 
            (T)LoadData(key, typeof(T));

        public object LoadData(string key, Type dtoType)
        {
            string json = PlayerPrefs.GetString(key, string.Empty);

            if (string.IsNullOrEmpty(json))
                throw new NullReferenceException(nameof(key));

            return JsonConvert.DeserializeObject(json, dtoType) ?? 
                   throw new NullReferenceException(nameof(json));
        }

        public void SaveData<T>(T dataModel, string key)
            where T : IDto
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