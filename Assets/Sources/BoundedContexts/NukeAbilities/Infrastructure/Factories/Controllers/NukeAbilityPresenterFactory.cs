using System;
using Sources.BoundedContexts.NukeAbilities.Controllers;
using Sources.BoundedContexts.NukeAbilities.Domain.Models;
using Sources.BoundedContexts.NukeAbilities.Presentation.Interfaces;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;
using Sources.InfrastructureInterfaces.Services.Repositories;

namespace Sources.BoundedContexts.NukeAbilities.Infrastructure.Factories.Controllers
{
    public class NukeAbilityPresenterFactory
    {
        private readonly IEntityRepository _entityRepository;
        private readonly IOverlapService _overlapService;

        public NukeAbilityPresenterFactory(
            IEntityRepository entityRepository,
            IOverlapService overlapService)
        {
            _entityRepository = entityRepository ?? 
                                throw new ArgumentNullException(nameof(entityRepository));
            _overlapService = overlapService ?? throw new ArgumentNullException(nameof(overlapService));
        }

        public NukeAbilityPresenter Create(INukeAbilityView view)
        {
            return new NukeAbilityPresenter(_entityRepository, view, _overlapService);
        }
    }
}