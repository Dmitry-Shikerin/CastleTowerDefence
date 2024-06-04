﻿using System;
using Sources.BoundedContexts.Abilities.Controllers;
using Sources.BoundedContexts.Abilities.Domain;
using Sources.BoundedContexts.Abilities.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.Abilities.Presentation.Implementation;
using Sources.BoundedContexts.Abilities.Presentation.Interfaces;

namespace Sources.BoundedContexts.Abilities.Infrastructure.Factories.Views
{
    public class AbilityApplierViewFactory
    {
        private readonly AbilityApplierPresenterFactory _presenterFactory;

        public AbilityApplierViewFactory(AbilityApplierPresenterFactory presenterFactory)
        {
            _presenterFactory = presenterFactory ?? throw new ArgumentNullException(nameof(presenterFactory));
        }

        public IAbilityApplierView Create(IAbilityApplier abilityApplier, AbilityApplierView view)
        {
            AbilityApplierPresenter presenter = _presenterFactory.Create(abilityApplier, view);
            view.Construct(presenter);
            
            return view;
        }
    }
}