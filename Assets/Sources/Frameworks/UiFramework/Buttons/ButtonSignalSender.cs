using System.Collections.Generic;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager.Components;
using Sirenix.OdinInspector;
using Sources.Frameworks.GameServices.DoozySignalBuses.Domain.Signals.Interfaces;
using Sources.Frameworks.UiFramework.Domain.Commands;
using UnityEngine;

namespace Sources.Frameworks.UiFramework.Buttons
{
    public class ButtonSignalSender : MonoBehaviour
    {
        [Required] [SerializeField] private UIButton _button;
        
        [TabGroup("Commands")] 
        [SerializeField] private List<ButtonCommandId> _onClickCommandId;
        [TabGroup("Commands")] [SerializeField]
        private List<ButtonCommandId> _enableCommandId;

        [TabGroup("Commands")] [SerializeField]
        private List<ButtonCommandId> _disableCommandId;

        private SignalStream _stream;

        public List<ButtonCommandId> OnClickCommandId => _onClickCommandId;
        public List<ButtonCommandId> EnableCommandId => _enableCommandId;
        public List<ButtonCommandId> DisableCommandId => _disableCommandId;

        public UIButton Button => _button;

        private void Awake() =>
            _stream = SignalStream.Get("ButtonCommand", "OnClick");

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