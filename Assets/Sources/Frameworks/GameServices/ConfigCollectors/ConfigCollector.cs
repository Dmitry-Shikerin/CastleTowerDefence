using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using MyAudios.MyUiFramework.Utils;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Sources.Frameworks.GameServices.ConfigCollectors
{
    public class ConfigCollector<T> : ScriptableObject
        where T : Config
    {
        [SerializeField] private List<Config> _configs = new List<Config>();
        [PropertySpace(10)] 
        [SerializeField] private string _addedConfigId;
        [ValueDropdown(nameof(GetRemovedId))]
        [SerializeField] private string _removedConfigId;

        public List<Config> Configs => _configs;

#if UNITY_EDITOR

        [Button]
        public void RemoveConfig()
        {
            Config config = Configs.FirstOrDefault(config => config.Id == _removedConfigId);
            AssetDatabase.RemoveObjectFromAsset(config);
            Configs.Remove(config);
            AssetDatabase.SaveAssets();
        }

        private List<string> GetRemovedId() =>
            _configs.Select(config => config.Id).ToList();

        [UsedImplicitly]
        [ResponsiveButtonGroup("Buttons")]
        private void CreateConfig()
        {
            if (string.IsNullOrWhiteSpace(_addedConfigId))
                return;

            if (string.IsNullOrEmpty(_addedConfigId))
                return;

            if (Configs.Any(config => config.Id == _addedConfigId))
            {
                EditorDialogUtils.ShowErrorDialog(
                    $"{typeof(T).Name} ConfigCollector",
                    $"This id not unique: {_addedConfigId}");

                return;
            }

            T config = CreateInstance<T>();
            config.SetParent(this);
            AssetDatabase.AddObjectToAsset(config, this);
            config.SetId(_addedConfigId);
            config.name = $"{_addedConfigId}_{typeof(T).Name}";
            Configs.Add(config);
            AssetDatabase.SaveAssets();
        }
#endif
    }
}