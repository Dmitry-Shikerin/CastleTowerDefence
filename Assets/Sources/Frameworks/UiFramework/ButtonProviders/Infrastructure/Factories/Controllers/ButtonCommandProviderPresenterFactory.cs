using System;
using Sources.Frameworks.UiFramework.ButtonProviders.Controllers;
using Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Services.Interfaces;
using Sources.Frameworks.UiFramework.ButtonProviders.Presentation.Interfaces;

namespace Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Factories.Controllers
{
    public class ButtonCommandProviderPresenterFactory
    {
        private readonly IUiButtonService _buttonService;

        public ButtonCommandProviderPresenterFactory(IUiButtonService buttonService)
        {
            _buttonService = buttonService ?? throw new ArgumentNullException(nameof(buttonService));
        }

        public ButtonCommandProviderPresenter Create(IButtonCommandProviderView view)
        {
            return new ButtonCommandProviderPresenter(view, _buttonService);
        }
    }
}