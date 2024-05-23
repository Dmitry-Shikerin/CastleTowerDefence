using System;
using System.Collections.Generic;
using Sources.Frameworks.UiFramework.Infrastructure.Factories.Views.Buttons;
using Sources.Frameworks.UiFramework.Infrastructure.Factories.Views.Forms;
using Sources.Frameworks.UiFramework.Presentation.Buttons;
using Sources.Frameworks.UiFramework.Presentation.Forms;
using Sources.Frameworks.UiFramework.Presentation.Views;
using Sources.Presentations.UI.Huds;

namespace Sources.Frameworks.UiFramework.Infrastructure.Factories.Services.Collectors
{
    public class UiCollectorFactory
    {
        private readonly UiButtonFactory _uiButtonFactory;
        private readonly UiViewFactory _uiViewFactory;
        private readonly IHud _hud;

        protected UiCollectorFactory(
            UiButtonFactory uiButtonFactory,
            UiViewFactory uiViewFactory,
            IHud hud)
        {
            _uiButtonFactory = uiButtonFactory ??
                                     throw new ArgumentNullException(nameof(uiButtonFactory));
            _uiViewFactory = uiViewFactory ?? throw new ArgumentNullException(nameof(uiViewFactory));
            _hud = hud ?? throw new ArgumentNullException(nameof(hud));
        }

        public void Create()
        {
            CreateFormButtons(_hud.UiCollector.UiFormButtons);
            CreateUiContainers(_hud.UiCollector.UiContainers);
        }

        private void CreateFormButtons(IEnumerable<UiButton> formButtons)
        {
            foreach (UiButton formButton in formButtons)
            {
                _uiButtonFactory.Create(formButton);
            }
        }

        private void CreateUiContainers(IEnumerable<UiView> containers)
        {
            foreach (UiView container in containers)
            {
                _uiViewFactory.Create(container);
            }
        }
    }
}