using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sources.Domain.Models.TextViewTypes;
using Sources.Frameworks.UiFramework.Domain.Constants;
using Sources.Frameworks.UiFramework.Extensions;
using Sources.Presentations.Views;
using Sources.PresentationsInterfaces.UI.Texts;
using TMPro;
using UnityEngine;

namespace Sources.Frameworks.UiFramework.Presentation.Texts
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UiText : View, IUiText
    {
        [DisplayAsString(false)] [HideLabel] 
        [SerializeField] private string _label = UiConstant.UiTextLabel;
        [SerializeField] private TextMeshProUGUI _tmpText;
        [TabGroup("Settings")] [SerializeField]
        private TextViewType _textViewType;
        [TabGroup("Ids")]
        [SerializeField] private string _textId;
        [TabGroup("Ids")] [ValueDropdown("GetDropdownValues")]
        [SerializeField] private string _localizationId;
        
        public TextViewType TextViewType => _textViewType;
        public bool IsHide { get; private set; }
        public string Id => _textId;

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

        [Button(ButtonSizes.Large)] [ButtonGroup("Settings")]
        public void AddTmpText() =>
            gameObject.AddComponent<TextMeshProUGUI>();

        [Button(ButtonSizes.Large)] [ButtonGroup("Settings")]
        public void SetTmpText() =>
            _tmpText = GetComponent<TextMeshProUGUI>();

        [TabGroup("Ids")]
        [Button(ButtonSizes.Large)]
        private void AddTextId()
        {
            var localizationIds = TextExtension.GetTranslateId();
        
            if(localizationIds.Contains(_textId))
                return;
            
            localizationIds.Add(_textId);
            
            _textId = "";
        }
        
        [UsedImplicitly]
        private List<string> GetDropdownValues() =>
            TextExtension.GetTranslateId();
    }
}