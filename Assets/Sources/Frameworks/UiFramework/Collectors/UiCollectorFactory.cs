using System;
using System.Collections.Generic;
using Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Factories.Views;
using Sources.Frameworks.UiFramework.ButtonProviders.Presentation.Implementation;
using Sources.Frameworks.UiFramework.Infrastructure.Factories.Views.Buttons;
using Sources.Frameworks.UiFramework.Infrastructure.Factories.Views.Forms;
using Sources.Frameworks.UiFramework.Presentation.Views;
using Sources.Presentations.UI.Huds;

namespace Sources.Frameworks.UiFramework.Collectors
{
    public class UiCollectorFactory
    {
        private readonly ButtonCommandProviderViewFactory _buttonCommandProviderViewFactory;
        private readonly IHud _hud;

        protected UiCollectorFactory(
            ButtonCommandProviderViewFactory buttonCommandProviderViewFactory,
            IHud hud)
        {
            _buttonCommandProviderViewFactory = buttonCommandProviderViewFactory ?? 
                                                throw new ArgumentNullException(nameof(buttonCommandProviderViewFactory));
            _hud = hud ?? throw new ArgumentNullException(nameof(hud));
        }

        public void Create()
        {
            CreateButtonCommandsProviders(_hud.UiCollector.UiFormButtons);
        }

        private void CreateButtonCommandsProviders(IEnumerable<ButtonCommandProviderView> formButtons)
        {
            foreach (ButtonCommandProviderView formButton in formButtons)
            {
                _buttonCommandProviderViewFactory.Create(formButton);
            }
        }
    }
}