using System.Collections.Generic;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager.Components;
using Sirenix.OdinInspector;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Constants;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Domain;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Signals;
using Sources.Frameworks.UiFramework.Core.Domain.Constants;
using UnityEngine;

namespace Sources.Frameworks.DoozyWrappers.SignalBuses.Presentation.Implementation
{
    [RequireComponent(typeof(UIButton))]
    public class UiButtonSignalSender : MonoBehaviour
    {
        [DisplayAsString(false)] [HideLabel] 
        [SerializeField] private string _labele = UiConstant.UiButtonSignalSenderLabel;
        [Space(10)]
        [Required] [SerializeField] private UIButton _button;
        [Space(10)]
        [SerializeField] private List<ButtonCommandId> _onClickCommandId;
        [Space(10)]
        [SerializeField] private List<ButtonCommandId> _enableCommandId;
        [Space(10)]
        [SerializeField] private List<ButtonCommandId> _disableCommandId;

        private SignalStream _stream;

        private void Awake() =>
            _stream = SignalStream.Get(StreamConst.ButtonCommand, StreamConst.OnClick);

        private void OnEnable() =>
            _button.onClickEvent.AddListener(SendSignal);

        private void OnDisable() =>
            _button.onClickEvent.RemoveListener(SendSignal);

        private void SendSignal() =>
            _stream.SendSignal(new ButtonCommandSignal(_onClickCommandId));

        [OnInspectorInit]
        private void SetButton()
        {
            if(_button == null)
                _button = GetComponent<UIButton>();
        }
    }
}