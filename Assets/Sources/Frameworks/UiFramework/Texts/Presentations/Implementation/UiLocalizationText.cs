using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using Sources.Frameworks.UiFramework.Core.Domain.Constants;
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

        [TabGroup("GetId")] 
        [ValueDropdown("GetDropdownValues")] [OnValueChanged("GetPhrase")]
        [SerializeField] private string _localizationId;

        [FormerlySerializedAs("_disableRussian")]
        [TabGroup("GetId")] 
        [SerializeField] private bool _disableTexts = true;
        [TabGroup("GetId")] 
        [TextArea(1, 20)] [Space(10)] [DisableIf("_disableTexts")]
        [SerializeField] private string _russianText;
        [TabGroup("GetId")] 
        [TextArea(1, 20)] [Space(10)] [DisableIf("_disableTexts")]
        [SerializeField] private string _englishText;
        [TabGroup("GetId")] 
        [TextArea(1, 20)] [Space(10)] [DisableIf("_disableTexts")]
        [SerializeField] private string _turkishText;

        [TabGroup("CreatePhrase")]
        [SerializeField] private string _textId;
        [TabGroup("CreatePhrase")] 
        [TextArea(1, 20)] [Space(10)]
        [SerializeField] private string _russian;
        [TabGroup("CreatePhrase")] 
        [TextArea(1, 20)] [Space(10)]
        [SerializeField] private string _english;
        [TabGroup("CreatePhrase")] 
        [TextArea(1, 20)] [Space(10)]
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

        [TabGroup("CreatePhrase")]
        [Button(ButtonSizes.Large)]
        private void CreatePhrase()
        {
            List<string> localizationIds = LocalizationDataBase.Instance.LocalizationIds;
        
            if(localizationIds.Contains(_textId))
                return;
            
            LocalizationPhrase phrase = LocalizationExtension.CreateLocalizationPhrase();
            phrase.SetId(_textId);
            phrase.SetRussian(_russian);
            phrase.SetEnglish(_english);
            phrase.SetTurkish(_turkish);
            
            LocalizationDataBase.Instance.AddAllPhrases();
        }
        
        [UsedImplicitly]
        private List<string> GetDropdownValues() =>
            LocalizationDataBase.Instance.LocalizationIds;

        [UsedImplicitly]
        private void GetPhrase()
        {
            var phrase = LocalizationDataBase.Instance.LocalizationPhrases
                .FirstOrDefault(phrase => phrase.LocalizationId == _localizationId);

            if (phrase == null)
                return;
            
            _russianText = phrase.Russian;
            _englishText = phrase.English;
            _turkishText = phrase.Turkish;
        }
    }
}