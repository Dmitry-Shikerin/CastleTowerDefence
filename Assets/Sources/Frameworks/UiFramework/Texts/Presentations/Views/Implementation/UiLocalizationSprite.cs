using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using Sources.Frameworks.UiFramework.Core.Domain.Constants;
using Sources.Frameworks.UiFramework.Core.Presentation.CommonTypes;
using Sources.Frameworks.UiFramework.Texts.Services.Localizations.Configs;
using Sources.Frameworks.UiFramework.Texts.Services.Localizations.Phrases;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Frameworks.UiFramework.Texts.Presentations.Views.Implementation
{
    [RequireComponent(typeof(Image))]
    public class UiLocalizationSprite : View, ISelfValidator
    {
        [DisplayAsString(false)] [HideLabel] 
        [SerializeField] private string _label = UiConstant.UiLocalizationSpriteLabel;

        [TabGroup("GetId", "Translations")] [Space(10)]
        [ValueDropdown("GetDropdownValues")] [OnValueChanged("GetPhrase")]
        [SerializeField] private string _localizationId;
        [TabGroup("GetId", "Translations")] [EnumToggleButtons] [Space(10)]
        [SerializeField] private Enable _disableTexts = Core.Presentation.CommonTypes.Enable.Disable;
        [TabGroup("GetId", "Translations")] 
        [Space(10)] 
        [DisableIf("_disableTexts", Core.Presentation.CommonTypes.Enable.Disable)]
        [PreviewField(250)]
        [SerializeField] private Sprite _russianSprite;
        [TabGroup("GetId", "Translations")] 
        [Space(10)]        
        [DisableIf("_disableTexts", Core.Presentation.CommonTypes.Enable.Disable)]
        [PreviewField(250)]
        [SerializeField] private Sprite _englishSprite;
        [TabGroup("GetId", "Translations")] 
        [Space(10)]         
        [DisableIf("_disableTexts", Core.Presentation.CommonTypes.Enable.Disable)]
        [PreviewField(250)]
        [SerializeField] private Sprite _turkishSprite;
        [Space(10)]
        [SerializeField] private Image _image;

        public bool IsHide { get; private set; }
        public string Id => _localizationId;

        public void Validate(SelfValidationResult result)
        {
            if (string.IsNullOrWhiteSpace(_localizationId))
            {
                result.AddError($"Localization Id is empty {gameObject.name}");
            }
        }

        private void Awake()
        {
            if (_image == null)
                throw new NullReferenceException(nameof(gameObject.name));
        }

        public void Enable() =>
            _image.enabled = true;

        public void Disable() =>
            _image.enabled = false;

        public void SetSprite(Sprite sprite) =>
            _image.sprite = sprite;

        [OnInspectorGUI]
        private void SetImage() =>
            _image = GetComponent<Image>();

        [UsedImplicitly]
        private List<string> GetDropdownValues() =>
            LocalizationDataBase.Instance.Phrases.Select(phrase => phrase.LocalizationId).ToList();

        [UsedImplicitly]
        private void GetPhrase()
        {
            LocalizationPhrase phrase = LocalizationDataBase.Instance.Phrases
                .FirstOrDefault(phrase => phrase.LocalizationId == _localizationId);

            if (phrase == null)
                return;
            
            _russianSprite = phrase.RussianSprite;
            _englishSprite = phrase.EnglishSprite;
            _turkishSprite = phrase.TurkishSprite;
        }

        [TabGroup("GetId", "Translations")]
        [ResponsiveButtonGroup("GetId/Translations/Get")] [UsedImplicitly]
        private void GetRussian() =>
            _image.sprite = _russianSprite;

        [TabGroup("GetId", "Translations")]
        [ResponsiveButtonGroup("GetId/Translations/Get")] [UsedImplicitly]
        private void GetEnglish() =>
            _image.sprite = _englishSprite;

        [TabGroup("GetId", "Translations")]
        [ResponsiveButtonGroup("GetId/Translations/Get")] [UsedImplicitly]
        private void GetTurkish() =>
            _image.sprite = _turkishSprite;
    }
}