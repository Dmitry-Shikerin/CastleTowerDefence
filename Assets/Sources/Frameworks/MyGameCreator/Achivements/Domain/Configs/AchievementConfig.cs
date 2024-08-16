using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.Frameworks.UiFramework.Texts.Services.Localizations.Configs;
using UnityEditor;
using UnityEngine;

namespace Sources.Frameworks.MyGameCreator.Achivements.Domain.Configs
{
    public class AchievementConfig : ScriptableObject
    {
        [HorizontalGroup("Split",0.17f, LabelWidth = 30)]
        [BoxGroup("Split/Left", ShowLabel = false)] 
        [HideLabel]
        [PreviewField(58, ObjectFieldAlignment.Center)]
        public Sprite Sprite;
        [BoxGroup("Split/Right", ShowLabel = false)]
        [LabelWidth(80)]
        [ValueDropdown("GetId")]
        public string Id;
        [BoxGroup("Split/Right")]
        [LabelWidth(80)]
        [ValueDropdown("GetLocalisationId")]
        public string TitleId;
        [BoxGroup("Split/Right")]
        [LabelWidth(80)]
        [ValueDropdown("GetLocalisationId")]
        public string DescriptionId;
        
        public AchievementConfigCollector Parent { get; set; }

        private IReadOnlyList<string> GetId() =>
            ModelId.AchievementModels;

#if UNITY_EDITOR
        [BoxGroup("Buttons")]
        [ResponsiveButtonGroup("Buttons/Buttons")]
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

        [BoxGroup("Buttons")]
        [ResponsiveButtonGroup("Buttons/Buttons")]
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