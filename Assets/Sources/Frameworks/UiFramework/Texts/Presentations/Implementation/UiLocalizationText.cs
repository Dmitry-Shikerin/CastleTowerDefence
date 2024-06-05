using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using Sources.Frameworks.UiFramework.Core.Domain.Constants;
using Sources.Frameworks.UiFramework.Core.Presentation.CommonTypes;
using Sources.Frameworks.UiFramework.Texts.Extensions;
using Sources.Frameworks.UiFramework.Texts.Presentations.Interfaces;
using Sources.Frameworks.UiFramework.Texts.Services.Localizations.Configs;
using Sources.Frameworks.UiFramework.Texts.Services.Localizations.Phrases;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sources.Frameworks.UiFramework.Texts.Presentations.Implementation
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UiLocalizationText : View, IUiLocalizationText
    {
        [DisplayAsString(false)] [HideLabel] 
        [SerializeField] private string _label = UiConstant.UiLocalizationTextLabel;

        [TabGroup("GetId", "Translations")] [Space(10)]
        [ValueDropdown("GetDropdownValues")] [OnValueChanged("GetPhrase")]
        [SerializeField] private string _localizationId;
        [TabGroup("GetId", "Translations")] [EnumToggleButtons] [Space(10)]
        [SerializeField] private Enable _disableTexts = Core.Presentation.CommonTypes.Enable.Disable;
        [TabGroup("GetId", "Translations")] 
        [TextArea(1, 20)] [Space(10)] 
        [DisableIf("_disableTexts", Core.Presentation.CommonTypes.Enable.Disable)]
        [SerializeField] private string _russianText;
        [TabGroup("GetId", "Translations")] 
        [TextArea(1, 20)] [Space(10)]        
        [DisableIf("_disableTexts", Core.Presentation.CommonTypes.Enable.Disable)]
        [SerializeField] private string _englishText;
        [TabGroup("GetId", "Translations")] 
        [TextArea(1, 20)] [Space(10)]         
        [DisableIf("_disableTexts", Core.Presentation.CommonTypes.Enable.Disable)]
        [SerializeField] private string _turkishText;
        [TabGroup("GetId", "CreatePhrase")] 
        [EnumToggleButtons] [Space(10)] [LabelText("TextId")]
        [SerializeField] private Enable _enableTextId;
        [TabGroup("GetId", "CreatePhrase")]
        [HideLabel] [ValidateInput("ValidateTextId", "TextId contains in DataBase")]
        [EnableIf("_enableTextId", Core.Presentation.CommonTypes.Enable.Enable)]
        [SerializeField] private string _textId;
        [TabGroup("GetId", "CreatePhrase")] 
        [EnumToggleButtons] [Space(10)] [LabelText("Russian")]
        [SerializeField] private Enable _enableRussian;
        [TabGroup("GetId", "CreatePhrase")]
        [TextArea(1, 20)] [HideLabel] 
        [EnableIf("_enableRussian", Core.Presentation.CommonTypes.Enable.Enable)]
        [SerializeField] private string _russian;
        [TabGroup("GetId", "CreatePhrase")] 
        [EnumToggleButtons] [Space(10)] [LabelText("English")]
        [SerializeField] private Enable _enableEnglish;
        [TabGroup("GetId", "CreatePhrase")]
        [TextArea(1, 20)] [HideLabel]
        [EnableIf("_enableEnglish", Core.Presentation.CommonTypes.Enable.Enable)]
        [SerializeField] private string _english;
        [TabGroup("GetId", "CreatePhrase")] 
        [EnumToggleButtons] [Space(10)] [LabelText("Turkish")]
        [SerializeField] private Enable _enableTurkish;
        [TabGroup("GetId", "CreatePhrase")]
        [TextArea(1, 20)] [HideLabel]
        [EnableIf("_enableTurkish", Core.Presentation.CommonTypes.Enable.Enable)]
        [SerializeField] private string _turkish;
        [Space(10)]
        [SerializeField] private TextMeshProUGUI _tmpText;

        public bool IsHide { get; private set; }
        public string Id => _localizationId;

        private void Awake()
        {
            if (_tmpText == null)
                throw new NullReferenceException(nameof(gameObject.name));
        }

        public void SetText(string text) =>
            _tmpText.text = text;

        public void SetTextColor(Color color) =>
            _tmpText.color = color;

        public void SetIsHide(bool isHide) =>
            IsHide = isHide;

        public async void SetClearColorAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (_tmpText.color.a > 0)
                {
                    _tmpText.color = Vector4.MoveTowards(
                        _tmpText.color, Vector4.zero, 0.01f);

                    await UniTask.Yield(cancellationToken);
                }

                IsHide = true;
            }
            catch (OperationCanceledException)
            {
                IsHide = true;
            }
        }

        public void Enable() =>
            _tmpText.enabled = true;

        public void Disable() =>
            _tmpText.enabled = false;
        
        [OnInspectorGUI]
        public void SetTmpText() =>
            _tmpText = GetComponent<TextMeshProUGUI>();

        [TabGroup("GetId", "CreatePhrase")]
        [Button(ButtonSizes.Large)]
        private void CreatePhrase()
        {
            LocalizationPhrase phrase = LocalizationDataBase.Instance.CreatePhrase(_textId);

            if (phrase == null)
                return;
             
            phrase.SetRussian(_russian);
            phrase.SetEnglish(_english);
            phrase.SetTurkish(_turkish);
        }
        
        [UsedImplicitly]
        private List<string> GetDropdownValues() =>
            LocalizationDataBase.Instance.LocalizationIds;

        [UsedImplicitly]
        private void GetPhrase()
        {
            var phrase = LocalizationDataBase.Instance.Phrases
                .FirstOrDefault(phrase => phrase.LocalizationId == _localizationId);

            if (phrase == null)
                return;
            
            _russianText = phrase.Russian;
            _englishText = phrase.English;
            _turkishText = phrase.Turkish;
        }

        [TabGroup("GetId", "Translations")]
        [ResponsiveButtonGroup("GetId/Translations/Get")] [UsedImplicitly]
        private void GetRussian() =>
            _tmpText.text = _russianText;

        [TabGroup("GetId", "Translations")]
        [ResponsiveButtonGroup("GetId/Translations/Get")] [UsedImplicitly]
        private void GetEnglish() =>
            _tmpText.text = _englishText;

        [TabGroup("GetId", "Translations")]
        [ResponsiveButtonGroup("GetId/Translations/Get")] [UsedImplicitly]
        private void GetTurkish() =>
            _tmpText.text = _turkishText;
        
        [UsedImplicitly]
        private bool ValidateTextId(string textId) =>
            LocalizationDataBase.Instance.LocalizationIds.Contains(textId) == false;
    }
}