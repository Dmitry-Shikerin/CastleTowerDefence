using System;
using Sources.BoundedContexts.NukeAbilities.Controllers;
using Sources.BoundedContexts.NukeAbilities.Domain.Models;
using Sources.BoundedContexts.NukeAbilities.Presentation.Interfaces;
using Sources.InfrastructureInterfaces.Services.Repositories;

namespace Sources.BoundedContexts.NukeAbilities.Infrastructure.Factories.Controllers
{
    public class NukeAbilityPresenterFactory
    {
        private readonly IEntityRepository _entityRepository;

        public NukeAbilityPresenterFactory(IEntityRepository entityRepository)
        {
            _entityRepository = entityRepository ?? 
                                throw new ArgumentNullException(nameof(entityRepository));
        }

        public NukeAbilityPresenter Create(INukeAbilityView view)
        {
            return new NukeAbilityPresenter(_entityRepository, view);
        }
    }
}