using Sources.BoundedContexts.Abilities.Controllers;
using Sources.BoundedContexts.Abilities.Domain;
using Sources.BoundedContexts.Abilities.Presentation.Interfaces;

namespace Sources.BoundedContexts.Abilities.Infrastructure.Factories.Controllers
{
    public class AbilityApplierPresenterFactory
    {
        public AbilityApplierPresenter Create(IAbilityApplier abilityApplier, IAbilityApplierView view)
        {
            return new AbilityApplierPresenter(abilityApplier, view);
        }
    }
}