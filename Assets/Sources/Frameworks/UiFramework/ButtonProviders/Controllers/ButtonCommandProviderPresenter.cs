using System;
using Sources.Controllers.Common;
using Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Services.Interfaces;
using Sources.Frameworks.UiFramework.ButtonProviders.Presentation.Interfaces;

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
            _buttonService.Handle(_view.EnableCommandId);
        }

        public override void Disable()
        {
            _view.Button.onClickEvent.RemoveListener(HandleCommand);
            _buttonService.Handle(_view.DisableCommandId);
        }

        private void HandleCommand()
        {
            _buttonService.Handle(_view.OnClickCommandId);
        }
    }
}