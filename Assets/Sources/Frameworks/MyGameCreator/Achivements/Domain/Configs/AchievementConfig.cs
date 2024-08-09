﻿using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.Frameworks.UiFramework.Texts.Services.Localizations.Configs;
using UnityEditor;
using UnityEngine;

namespace Sources.Frameworks.MyGameCreator.Achivements.Domain.Configs
{
    [CreateAssetMenu(
        fileName = "AchievementConfig", 
        menuName = "Configs/Achievements/AchievementConfig", 
        order = 51)]
    public class AchievementConfig : ScriptableObject
    {
        [ValueDropdown("GetId")]
        public string Id;
        [ValueDropdown("GetLocalisationId")]
        public string TitleId;
        [ValueDropdown("GetLocalisationId")]
        public string DescriptionId;
        public Sprite Sprite;
        
        public AchievementConfigCollector Parent { get; set; }

        private IReadOnlyList<string> GetId() =>
            ModelId.AchievementModels;

#if UNITY_EDITOR
        [PropertySpace(10)]
        [Button(ButtonSizes.Medium)]
        private void Rename()
        {
            if (string.IsNullOrEmpty(Id))
                return;

            if (string.IsNullOrWhiteSpace(Id))
                return;
            
            Parent.CreateConfig(Id, TitleId, DescriptionId, Sprite);
            Parent.RemoveConfig(this);
        }

        [PropertySpace(10)]
        [Button(ButtonSizes.Medium)]
        private void Remove() =>
            Parent.RemoveConfig(this);

        private List<string> GetLocalisationId() =>
            LocalizationDataBase.Instance.Phrases
                .Select(phrase => phrase.LocalizationId)
                .ToList();
#endif
    }
}