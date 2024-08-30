using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sources.Frameworks.UiFramework.Core.Domain.Constants;
using Sources.Frameworks.UiFramework.Core.Presentation.CommonTypes;
using Sources.Frameworks.UiFramework.Texts.Extensions;
using Sources.Frameworks.UiFramework.Texts.Services.Localizations.Configs;
using UnityEditor;
using UnityEngine;

namespace Sources.Frameworks.UiFramework.Texts.Services.Localizations.Phrases
{
    public class LocalizationPhrase : ScriptableObject
    {
        [DisplayAsString(false)] [HideLabel] 
        [SerializeField] private string _headere = UiConstant.UiLocalizationPhraseLabel;

        [EnumToggleButtons] [LabelText("Parent")]
        [SerializeField] private Enable _editParent;
        [DisableIf("_editParent", Enable.Disable)]
        [SerializeField] private LocalizationDataBase _parent;
        [DisableIf("_editParent", Enable.Disable)]
        [SerializeField] private LocalizationScope _scope;

        [ValueDropdown("GetDropdownValues")] [Space(10)]
        [SerializeField] private string _localizationId;
        
        [SerializeField] private string _textId;

        [FoldoutGroup("Russian")] [EnumToggleButtons] [LabelText("Russian")]
        [SerializeField] private Enable _editRussian;
        [FoldoutGroup("Russian")] [TextArea(1, 20)] [HideLabel]
        [EnableIf("_editRussian", Enable.Enable)]
        [SerializeField] private string _russian;
        [FoldoutGroup("Russian")]
        [EnableIf("_editRussian", Enable.Enable)]
        [PreviewField(200)] [HideLabel]
        [SerializeField] private Sprite _russianSprite;

        [FoldoutGroup("English")] [EnumToggleButtons] [LabelText("English")]
        [SerializeField] private Enable _editEnglish;
        [EnableIf("_editEnglish", Enable.Enable)] 
        [FoldoutGroup("English")] [TextArea(1, 20)] [HideLabel]
        [SerializeField] private string _english;
        [EnableIf("_editEnglish", Enable.Enable)]
        [FoldoutGroup("English")]
        [PreviewField(200)] [HideLabel]
        [SerializeField] private Sprite _englishSprite;

        [FoldoutGroup("Turkish")] [EnumToggleButtons] [LabelText("Turkish")]
        [SerializeField] private Enable _editTurkish;
        [EnableIf("_editTurkish", Enable.Enable)] 
        [FoldoutGroup("Turkish")] [TextArea(1, 20)] [HideLabel]
        [SerializeField] private string _turkish;
        [EnableIf("_editTurkish", Enable.Enable)]
        [FoldoutGroup("Turkish")]
        [PreviewField(200)] [HideLabel]
        [SerializeField] private Sprite _turkishSprite;

        public string LocalizationId => _localizationId;
        public string Russian => _russian;
        public Sprite RussianSprite => _russianSprite;
        public string English => _english;
        public Sprite EnglishSprite => _englishSprite;
        public string Turkish => _turkish;
        public Sprite TurkishSprite => _turkishSprite;
        
        public LocalizationScope Scope => _scope;
        
        public void SetScope(LocalizationScope scope) =>
            _scope = scope ?? throw new UnityException("Scope is null");
        
        public void SetId(string id) =>
            _localizationId = id;
        
        public void SetRussian(string russian) =>
            _russian = russian;
        
        public void SetEnglish(string english) =>
            _english = english;
        
        public void SetTurkish(string turkish) =>
            _turkish = turkish;
        
        public void SetDataBase(LocalizationDataBase parent) =>
            _parent = parent ?? throw new UnityException("Parent is null");

        [Button(ButtonSizes.Large)] 
        [ResponsiveButtonGroup]
        private void ChangeName()
        {
            if (string.IsNullOrWhiteSpace(_localizationId))
                return;

            LocalizationExtension.RenameAsset(this, _localizationId);
        }

        [Button(ButtonSizes.Large)] 
        [ResponsiveButtonGroup]
        private void AddTextId()
        {
#if UNITY_EDITOR
            var localizationIds = LocalizationExtension.GetTranslateId();

            if (localizationIds.Contains(_textId))
                return;

            _localizationId = _textId;
            AssetDatabase.SaveAssets();
#endif
        }

        [Button(ButtonSizes.Large)]
        [ResponsiveButtonGroup]
        private void Remove()
        {
            _parent.RemovePhrase(this);
        }

        [UsedImplicitly]
        private List<string> GetDropdownValues() =>
            LocalizationDataBase.Instance.Phrases.Select(phrase => phrase.LocalizationId).ToList();
    }
}