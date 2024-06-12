using Sources.BoundedContexts.BurnAbilities.Controllers;
using Sources.BoundedContexts.BurnAbilities.Domain;
using Sources.BoundedContexts.BurnAbilities.Presentation.Interfaces;

namespace Sources.BoundedContexts.BurnAbilities.Infrastructure.Factories.Controllers
{
    public class BurnAbilityPresenterFactory
    {
        public BurnAbilityPresenter Create(BurnAbility burnAbility, IBurnAbilityView view)
        {
            return new BurnAbilityPresenter(burnAbility, view);
        }
    }
}