using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sources.Frameworks.UiFramework.Core.Domain.Constants;
using Sources.Frameworks.UiFramework.Core.Presentation.CommonTypes;
using Sources.Frameworks.UiFramework.Texts.Extensions;
using UnityEngine;

namespace Sources.Frameworks.UiFramework.Texts.Services.Localizations.Phrases
{
    [CreateAssetMenu(fileName = "LocalizationPhrase", menuName = "Configs/LocalizationPhrase", order = 51)]
    public class LocalizationPhrase : ScriptableObject
    {
        [DisplayAsString(false)] [HideLabel] 
        [SerializeField] private string _headere = UiConstant.UiLocalizationPhraseLabel;
        
        [ValueDropdown("GetDropdownValues")] [Space(10)]
        [SerializeField] private string _localizationId;

        [SerializeField] private string _textId;

        [FoldoutGroup("Russian")] [EnumToggleButtons] [LabelText("Russian")]
        [SerializeField] private Enable _editRussian;

        [FoldoutGroup("Russian")] [TextArea(1, 20)] [HideLabel]
        [EnableIf("_editRussian", Enable.Enable)]
        [SerializeField] private string _russian;

        [FoldoutGroup("English")] [EnumToggleButtons] [LabelText("English")]
        [SerializeField] private Enable _editEnglish;

        [EnableIf("_editEnglish", Enable.Enable)] 
        [FoldoutGroup("English")] [TextArea(1, 20)] [HideLabel]
        [SerializeField] private string _english;

        [FoldoutGroup("Turkish")] [EnumToggleButtons] [LabelText("Turkish")]
        [SerializeField] private Enable _editTurkish;

        [EnableIf("_editTurkish", Enable.Enable)] 
        [FoldoutGroup("Turkish")] [TextArea(1, 20)] [HideLabel]
        [SerializeField] private string _turkish;

        public string LocalizationId => _localizationId;
        public string Russian => _russian;
        public string English => _english;
        public string Turkish => _turkish;
        
        public void SetId(string id) =>
            _localizationId = id;
        
        public void SetRussian(string russian) =>
            _russian = russian;
        
        public void SetEnglish(string english) =>
            _english = english;
        
        public void SetTurkish(string turkish) =>
            _turkish = turkish;

        [Button(ButtonSizes.Large)] [ResponsiveButtonGroup]
        private void ChangeName()
        {
            if (string.IsNullOrWhiteSpace(_localizationId))
                return;

            LocalizationExtension.RenameAsset(this, _localizationId);
        }

        [Button(ButtonSizes.Large)] [ResponsiveButtonGroup]
        private void AddTextId()
        {
            var localizationIds = LocalizationExtension.GetTranslateId();

            if (localizationIds.Contains(_textId))
                return;

            localizationIds.Add(_textId);
            localizationIds.Sort();

            _textId = "";
        }

        [UsedImplicitly]
        private List<string> GetDropdownValues() =>
            LocalizationExtension.GetTranslateId();
    }
}