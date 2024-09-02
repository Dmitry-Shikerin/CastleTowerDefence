using System;
using Sources.BoundedContexts.NukeAbilities.Controllers;
using Sources.BoundedContexts.NukeAbilities.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.NukeAbilities.Presentation.Implementation;
using Sources.BoundedContexts.NukeAbilities.Presentation.Interfaces;

namespace Sources.BoundedContexts.NukeAbilities.Infrastructure.Factories.Views
{
    public class NukeAbilityViewFactory
    {
        private readonly NukeAbilityPresenterFactory _presenterFactory;

        public NukeAbilityViewFactory(NukeAbilityPresenterFactory presenterFactory)
        {
            _presenterFactory = presenterFactory ?? throw new ArgumentNullException(nameof(presenterFactory));
        }
        
        public INukeAbilityView Create(NukeAbilityView view)
        {
            NukeAbilityPresenter presenter = _presenterFactory.Create(view);
            view.Construct(presenter);

            return view;
        }
    }
}