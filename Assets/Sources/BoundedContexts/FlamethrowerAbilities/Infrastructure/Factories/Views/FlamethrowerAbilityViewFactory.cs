using System;
using JetBrains.Annotations;
using Sources.BoundedContexts.FlamethrowerAbilities.Controllers;
using Sources.BoundedContexts.FlamethrowerAbilities.Domain.Models;
using Sources.BoundedContexts.FlamethrowerAbilities.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.FlamethrowerAbilities.Presentation.Implementation;
using Sources.BoundedContexts.FlamethrowerAbilities.Presentation.Interfaces;

namespace Sources.BoundedContexts.FlamethrowerAbilities.Infrastructure.Factories.Views
{
    public class FlamethrowerAbilityViewFactory
    {
        private readonly FlamethrowerAbilityPresenterFactory _presenterFactory;

        public FlamethrowerAbilityViewFactory(FlamethrowerAbilityPresenterFactory presenterFactory)
        {
            _presenterFactory = presenterFactory ?? throw new ArgumentNullException(nameof(presenterFactory));
        }

        public IFlamethrowerAbilityView Create(FlamethrowerAbility flamethrowerAbility, FlamethrowerAbilityView view)
        {
            FlamethrowerAbilityPresenter presenter = _presenterFactory.Create(flamethrowerAbility, view);
            view.Construct(presenter);

            return view;
        }
    }
}