using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sources.Frameworks.GameServices.Loads.Domain.Constant;
using Sources.Frameworks.MyGameCreator.Achievements.Domain.Models;
using Sources.Frameworks.UiFramework.Texts.Services.Localizations.Configs;
using UnityEditor;
using UnityEngine;

namespace Sources.Frameworks.MyGameCreator.Achievements.Domain.Configs
{
    public class AchievementConfig : ScriptableObject
    {
        [HorizontalGroup("Split",0.17f, LabelWidth = 30)]
        [BoxGroup("Split/Left", ShowLabel = false)] 
        [HideLabel]
        [PreviewField(58, ObjectFieldAlignment.Center)]
        public Sprite Sprite;
        [BoxGroup("Split/Right", ShowLabel = false)]
        [HorizontalGroup("Split/Right/Id", 0.93f)]
        [LabelWidth(80)]
        [ValueDropdown("GetId")]
        public string Id;
        [BoxGroup("Split/Right")]
        [HorizontalGroup("Split/Right/TitleId", 0.93f)]
        [LabelWidth(80)]
        [ValueDropdown("GetLocalisationId")]
        public string TitleId;
        [BoxGroup("Split/Right")]
        [HorizontalGroup("Split/Right/DescriptionId", 0.93f)]
        [LabelWidth(80)]
        [ValueDropdown("GetLocalisationId")]
        public string DescriptionId;
        
        public AchievementConfigCollector Parent { get; set; }

        private IReadOnlyList<string> GetId() =>
            ModelId.GetIds<Achievement>();

        [HideLabel]
        [HorizontalGroup("Split/Right/Id")]
        [Button(SdfIconType.Search)]
        private void PingId()
        {
            // Selection.activeObject = LocalizationDataBase.Instance.GetPhrase(Id);
        }

        [HideLabel]
        [HorizontalGroup("Split/Right/TitleId")]
        [Button(SdfIconType.Search)]
        private void PingTitleId()
        {
#if UNITY_EDITOR
            
            Selection.activeObject = LocalizationDataBase.Instance.GetPhrase(TitleId);
#endif
        }

        [HideLabel]
        [HorizontalGroup("Split/Right/DescriptionId")]
        [Button(SdfIconType.Search)]
        private void PingDescriptionId()
        {
#if UNITY_EDITOR
            
            Selection.activeObject = LocalizationDataBase.Instance.GetPhrase(DescriptionId);
#endif
        }

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