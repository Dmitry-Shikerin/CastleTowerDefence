using System;
using Sources.Controllers.Common;
using Sources.Frameworks.UiFramework.ButtonProviders.Presentation.Interfaces;
using Sources.Frameworks.UiFramework.Buttons.Services.Interfaces;

namespace Sources.Frameworks.UiFramework.ButtonProviders.Controllers
{
    public class ButtonCommandProviderPresenter : PresenterBase
    {
        private readonly IButtonCommandProviderView _view;
        private readonly IUiButtonService _buttonService;

        public ButtonCommandProviderPresenter(
            IButtonCommandProviderView view, 
            IUiButtonService buttonService)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _buttonService = buttonService ?? throw new ArgumentNullException(nameof(buttonService));
        }
        
        public override void Enable()
        {
            _view.Button.onClickEvent.AddListener(HandleCommand);
            _buttonService.Handle(_view.EnableCommandId, null);
        }

        public override void Disable()
        {
            _view.Button.onClickEvent.RemoveListener(HandleCommand);
            _buttonService.Handle(_view.DisableCommandId, null);
        }

        private void HandleCommand()
        {
            _buttonService.Handle(_view.OnClickCommandId, null);
        }
    }
}