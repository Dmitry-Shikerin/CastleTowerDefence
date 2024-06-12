using System;
using Sources.BoundedContexts.BurnAbilities.Controllers;
using Sources.BoundedContexts.BurnAbilities.Domain;
using Sources.BoundedContexts.BurnAbilities.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.BurnAbilities.Presentation.Implementation;
using Sources.BoundedContexts.BurnAbilities.Presentation.Interfaces;

namespace Sources.BoundedContexts.BurnAbilities.Infrastructure.Factories.Views
{
    public class BurnAbilityViewFactory
    {
        private readonly BurnAbilityPresenterFactory _presenterFactory;

        public BurnAbilityViewFactory(BurnAbilityPresenterFactory presenterFactory)
        {
            _presenterFactory = presenterFactory ?? throw new ArgumentNullException(nameof(presenterFactory));
        }

        public IBurnAbilityView Create(BurnAbility burnAbility, BurnAbilityView view)
        {
            BurnAbilityPresenter presenter = _presenterFactory.Create(burnAbility, view);
            view.Construct(presenter);
            
            return view;
        }
    }
}