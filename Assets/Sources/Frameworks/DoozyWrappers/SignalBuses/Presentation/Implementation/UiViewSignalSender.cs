using System.Collections.Generic;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Constants;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Signals;
using Sources.Frameworks.UiFramework.Core.Domain.Constants;
using Sources.Frameworks.UiFramework.Views.Domain;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sources.Frameworks.DoozyWrappers.SignalBuses.Presentation.Implementation
{
    public class UiViewSignalSender : MonoBehaviour
    {
        [DisplayAsString(false)] [HideLabel] 
        [SerializeField] private string _labele = UiConstant.UiViewSignalSenderLabel;
        [FormerlySerializedAs("_button")]
        [Space(10)]
        [Required] [SerializeField] private UIView _view;
        [Space(10)]
        [SerializeField] private List<FormCommandId> _enableCommandId;
        [Space(10)]
        [SerializeField] private List<FormCommandId> _disableCommandId;
        
        private SignalStream _enableStream;
        private SignalStream _disableStream;

        private void Awake()
        {
            _enableStream = SignalStream.Get(StreamConst.ViewCommandCategory, StreamConst.ShowViewCommand);
            _disableStream = SignalStream.Get(StreamConst.ViewCommandCategory, StreamConst.HideViewCommand);
        }

        private void OnEnable()
        {
            _view.OnShowCallback.Event.AddListener(OnShow);
            _view.OnHideCallback.Event.AddListener(OnHide);
        }

        private void OnDisable()
        {
            _view.OnShowCallback.Event.AddListener(OnShow);
            _view.OnHideCallback.Event.RemoveListener(OnHide);
        }
        
        [OnInspectorInit]
        private void SetView()
        {
            if(_view == null)
                _view = GetComponent<UIView>();
        }

        private void OnShow() =>
            _enableStream.SendSignal(new ShowViewCommandSignal(_enableCommandId));

        private void OnHide() =>
            _disableStream.SendSignal(new HideViewCommandSignal(_disableCommandId));
    }
}