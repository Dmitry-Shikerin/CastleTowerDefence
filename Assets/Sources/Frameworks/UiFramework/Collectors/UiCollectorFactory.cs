using System;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.UI.Huds;
using Sources.Frameworks.UiFramework.Views.Presentations.Implementation;

namespace Sources.Frameworks.UiFramework.Collectors
{
    public class UiCollectorFactory
    {
        private readonly IHud _hud;

        protected UiCollectorFactory(
            UiCollector uiCollector)
        {
        }

        public void Create()
        {
        }
    }
}