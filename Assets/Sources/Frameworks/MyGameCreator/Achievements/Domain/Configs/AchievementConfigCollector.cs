using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sources.Frameworks.GameServices.Loads.Domain.Constant;
using Sources.Frameworks.MyGameCreator.Achievements.Domain.Models;
using Sources.Frameworks.UiFramework.Texts.Services.Localizations.Configs;
using UnityEditor;
using UnityEngine;

namespace Sources.Frameworks.MyGameCreator.Achievements.Domain.Configs
{
    [CreateAssetMenu(
        fileName = "AchievementConfigCollector", 
        menuName = "Configs/Achievements/AchievementConfigCollector", 
        order = 51)]
    public class AchievementConfigCollector : ScriptableObject
    {
        [field: SerializeField] public List<AchievementConfig> Configs { get; private set; }
        [PropertySpace(10)]
        [ValueDropdown("GetId")]
        [SerializeField] private string _configId;
        [ValueDropdown("GetLocalisationId")]
        [SerializeField] private string _titleId;
        [ValueDropdown("GetLocalisationId")]
        [SerializeField] private string _descriptionId;
        [SerializeField] private Sprite _sprite;
        
#if UNITY_EDITOR
        public void RemoveConfig(AchievementConfig wave)
        {
            AssetDatabase.RemoveObjectFromAsset(wave);
            Configs.Remove(wave);
            AssetDatabase.SaveAssets();
        }

        private IReadOnlyList<string> GetId() =>
            ModelId.GetIds<Achievement>();

        [UsedImplicitly]
        [ResponsiveButtonGroup("Buttons")]
        private void CreateConfig()
        {
            if (string.IsNullOrWhiteSpace(_configId))
                return;

            if (string.IsNullOrEmpty(_configId))
                return;

            if (Configs.Any(config => config.Id == _configId))
            {
                Debug.Log($"this id not unique: {_configId}");
                return;
            }
            
            AchievementConfig achievementConfig = CreateInstance<AchievementConfig>();
            achievementConfig.Parent = this;
            AssetDatabase.AddObjectToAsset(achievementConfig, this);
            achievementConfig.Id = _configId;
            achievementConfig.name = $"{_configId}_AchievementConfig";
            achievementConfig.TitleId = _titleId;
            achievementConfig.DescriptionId = _descriptionId;
            achievementConfig.Sprite = _sprite;
            Configs.Add(achievementConfig);
            AssetDatabase.SaveAssets();
        }

        public void CreateConfig(string id, string titleId, string descriptionId, Sprite sprite)
        {
            _configId = id;
            _titleId = titleId;
            _descriptionId = descriptionId;
            _sprite = sprite;
            CreateConfig();
        }
        
        private List<string> GetLocalisationId() =>
            LocalizationDataBase.Instance.Phrases
                .Select(phrase => phrase.LocalizationId)
                .ToList();
#endif
    }
}