using System.Collections.Generic;
using Doozy.Runtime.UIManager.Components;
using Sirenix.OdinInspector;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using Sources.Frameworks.UiFramework.ButtonProviders.Controllers;
using Sources.Frameworks.UiFramework.ButtonProviders.Presentation.Interfaces;
using Sources.Frameworks.UiFramework.Domain.Commands;
using Sources.Presentations.Views;
using UnityEngine;

namespace Sources.Frameworks.UiFramework.ButtonProviders.Presentation.Implementation
{
    [RequireComponent(typeof(UIButton))]
    public class ButtonCommandProviderView : PresentableView<ButtonCommandProviderPresenter>, 
        IButtonCommandProviderView
    {
        [Required] [SerializeField] private UIButton _button;
        
        [TabGroup("Commands")] 
        [SerializeField] private List<ButtonCommandId> _onClickCommandId;
        [TabGroup("Commands")] [SerializeField]
        private List<ButtonCommandId> _enableCommandId;
        [TabGroup("Commands")] [SerializeField]
        private List<ButtonCommandId> _disableCommandId;

        public List<ButtonCommandId> OnClickCommandId => _onClickCommandId;
        public List<ButtonCommandId> EnableCommandId => _enableCommandId;
        public List<ButtonCommandId> DisableCommandId => _disableCommandId;
        
        public UIButton Button => _button;

        [OnInspectorInit]
        private void SetButton()
        {
            if(_button == null)
                _button = GetComponent<UIButton>();
        }
    }
}