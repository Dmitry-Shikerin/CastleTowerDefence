using Sources.BoundedContexts.NukeAbilities.Controllers;
using Sources.BoundedContexts.NukeAbilities.Domain.Models;
using Sources.BoundedContexts.NukeAbilities.Presentation.Interfaces;

namespace Sources.BoundedContexts.NukeAbilities.Infrastructure.Factories.Controllers
{
    public class NukeAbilityPresenterFactory
    {
        public NukeAbilityPresenter Create(NukeAbility nukeAbility, INukeAbilityView view)
        {
            return new NukeAbilityPresenter(nukeAbility, view);
        }
    }
}