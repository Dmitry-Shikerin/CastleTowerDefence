using System;
using JetBrains.Annotations;
using Sources.BoundedContexts.NukeAbilities.Controllers;
using Sources.BoundedContexts.NukeAbilities.Domain.Models;
using Sources.BoundedContexts.NukeAbilities.Presentation.Interfaces;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;
using Sources.InfrastructureInterfaces.Services.Cameras;
using Sources.InfrastructureInterfaces.Services.Repositories;

namespace Sources.BoundedContexts.NukeAbilities.Infrastructure.Factories.Controllers
{
    public class NukeAbilityPresenterFactory
    {
        private readonly IEntityRepository _entityRepository;
        private readonly IOverlapService _overlapService;
        private readonly ICameraService _cameraService;

        public NukeAbilityPresenterFactory(
            IEntityRepository entityRepository,
            IOverlapService overlapService,
            ICameraService cameraService)
        {
            _entityRepository = entityRepository ?? 
                                throw new ArgumentNullException(nameof(entityRepository));
            _overlapService = overlapService ?? throw new ArgumentNullException(nameof(overlapService));
            _cameraService = cameraService ?? throw new ArgumentNullException(nameof(cameraService));
        }

        public NukeAbilityPresenter Create(INukeAbilityView view)
        {
            return new NukeAbilityPresenter(
                _entityRepository, view, _overlapService, _cameraService);
        }
    }
}