using System;
using Sources.BoundedContexts.FlamethrowerAbilities.Controllers;
using Sources.BoundedContexts.FlamethrowerAbilities.Domain.Models;
using Sources.BoundedContexts.FlamethrowerAbilities.Presentation.Interfaces;
using Sources.InfrastructureInterfaces.Services.Repositories;

namespace Sources.BoundedContexts.FlamethrowerAbilities.Infrastructure.Factories.Controllers
{
    public class FlamethrowerAbilityPresenterFactory
    {
        private readonly IEntityRepository _entityRepository;

        public FlamethrowerAbilityPresenterFactory(IEntityRepository entityRepository)
        {
            _entityRepository = entityRepository ?? 
                                throw new ArgumentNullException(nameof(entityRepository));
        }

        public FlamethrowerAbilityPresenter Create(IFlamethrowerAbilityView view)
        {
            return new FlamethrowerAbilityPresenter(_entityRepository, view);
        }
    }
}