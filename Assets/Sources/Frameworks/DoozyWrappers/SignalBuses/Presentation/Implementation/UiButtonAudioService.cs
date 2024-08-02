using Doozy.Runtime.UIManager.Components;
using Sirenix.OdinInspector;
using Sources.Frameworks.UiFramework.AudioSources.Presentations.Implementation.Types;
using Sources.Frameworks.UiFramework.Core.Domain.Constants;
using UnityEngine;

namespace Sources.Frameworks.DoozyWrappers.SignalBuses.Presentation.Implementation
{
    [RequireComponent(typeof(UIButton))]
    public class UiButtonAudioService : AudioServiceSignalSender
    {
        [DisplayAsString(false)] [HideLabel] 
        [SerializeField] private string _labele = UiConstant.UiButtonAudioServiceLabel;
        [Space(10)]
        [SerializeField] private AudioClipId _onClickAudioClip; 
        [Space(10)]
        [SerializeField] private UIButton _button;
        
        protected override void OnAfterEnable() =>
            _button.onClickEvent.AddListener(PlayClickSound);

        protected override void OnAfterDisable() =>
            _button.onClickEvent.RemoveListener(PlayClickSound);

        [OnInspectorInit]
        private void Set() =>
            _button = GetComponent<UIButton>();

        private void PlayClickSound() =>
            SendSignal(_onClickAudioClip);
    }
}