using System;
using Sources.Frameworks.UiFramework.ButtonProviders.Controllers;
using Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Factories.Controllers;
using Sources.Frameworks.UiFramework.ButtonProviders.Presentation.Implementation;
using Sources.Frameworks.UiFramework.ButtonProviders.Presentation.Interfaces;

namespace Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Factories.Views
{
    public class ButtonCommandProviderViewFactory
    {
        private readonly ButtonCommandProviderPresenterFactory _presenterFactory;

        public ButtonCommandProviderViewFactory(ButtonCommandProviderPresenterFactory presenterFactory)
        {
            _presenterFactory = presenterFactory ?? throw new ArgumentNullException(nameof(presenterFactory));
        }

        public IButtonCommandProviderView Create(ButtonCommandProviderView view)
        {
            ButtonCommandProviderPresenter presenter = _presenterFactory.Create(view);
            view.Construct(presenter);
            
            return view;
        }
}
}