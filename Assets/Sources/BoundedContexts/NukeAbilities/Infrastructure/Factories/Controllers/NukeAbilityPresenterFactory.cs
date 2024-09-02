using System;
using Sources.BoundedContexts.NukeAbilities.Controllers;
using Sources.BoundedContexts.NukeAbilities.Presentation.Interfaces;
using Sources.Frameworks.GameServices.Cameras.Infrastructure.Services.Interfaces;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;
using Sources.Frameworks.MyAudio_master.MyAudio.Soundy.Sources.Soundies.Infrastructure.Interfaces;

namespace Sources.BoundedContexts.NukeAbilities.Infrastructure.Factories.Controllers
{
    public class NukeAbilityPresenterFactory
    {
        private readonly IEntityRepository _entityRepository;
        private readonly IOverlapService _overlapService;
        private readonly ISoundyService _soundyService;
        private readonly ICameraService _cameraService;

        public NukeAbilityPresenterFactory(
            IEntityRepository entityRepository,
            IOverlapService overlapService,
            ISoundyService soundyService,
            ICameraService cameraService)
        {
            _entityRepository = entityRepository ?? 
                                throw new ArgumentNullException(nameof(entityRepository));
            _overlapService = overlapService ?? throw new ArgumentNullException(nameof(overlapService));
            _soundyService = soundyService ?? throw new ArgumentNullException(nameof(soundyService));
            _cameraService = cameraService ?? throw new ArgumentNullException(nameof(cameraService));
        }

        public NukeAbilityPresenter Create(INukeAbilityView view)
        {
            return new NukeAbilityPresenter(
                _entityRepository, view, _overlapService, _soundyService, _cameraService);
        }
    }
}