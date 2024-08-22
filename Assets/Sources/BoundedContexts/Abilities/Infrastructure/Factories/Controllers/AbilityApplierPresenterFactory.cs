using System;
using Sources.BoundedContexts.Abilities.Controllers;
using Sources.BoundedContexts.Abilities.Presentation.Interfaces;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;

namespace Sources.BoundedContexts.Abilities.Infrastructure.Factories.Controllers
{
    public class AbilityApplierPresenterFactory
    {
        private readonly IEntityRepository _entityRepository;

        public AbilityApplierPresenterFactory(IEntityRepository entityRepository)
        {
            _entityRepository = entityRepository ?? 
                                throw new ArgumentNullException(nameof(entityRepository));
        }

        public AbilityApplierPresenter Create(string abilityId, IAbilityApplierView view)
        {
            return new AbilityApplierPresenter(_entityRepository, abilityId, view);
        }
    }
}