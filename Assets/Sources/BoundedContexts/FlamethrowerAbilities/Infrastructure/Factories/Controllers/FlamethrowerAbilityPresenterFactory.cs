using Sources.BoundedContexts.FlamethrowerAbilities.Controllers;
using Sources.BoundedContexts.FlamethrowerAbilities.Domain.Models;
using Sources.BoundedContexts.FlamethrowerAbilities.Presentation.Interfaces;

namespace Sources.BoundedContexts.FlamethrowerAbilities.Infrastructure.Factories.Controllers
{
    public class FlamethrowerAbilityPresenterFactory
    {
        public FlamethrowerAbilityPresenter Create(
            FlamethrowerAbility flamethrowerAbility,
            IFlamethrowerAbilityView view)
        {
            return new FlamethrowerAbilityPresenter(flamethrowerAbility, view);
        }
    }
}