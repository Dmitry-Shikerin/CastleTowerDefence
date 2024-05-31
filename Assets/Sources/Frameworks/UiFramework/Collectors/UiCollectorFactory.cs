﻿using System;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.UI.Huds;

namespace Sources.Frameworks.UiFramework.Collectors
{
    public class UiCollectorFactory
    {
        private readonly IHud _hud;

        protected UiCollectorFactory(
            IHud hud)
        {
            _hud = hud ?? throw new ArgumentNullException(nameof(hud));
        }

        public void Create()
        {
        }
    }
}