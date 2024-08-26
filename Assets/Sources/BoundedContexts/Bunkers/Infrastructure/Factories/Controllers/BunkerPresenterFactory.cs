using System;
using Sources.BoundedContexts.Bunkers.Controllers;
using Sources.BoundedContexts.Bunkers.Presentation.Interfaces;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;
using Sources.Frameworks.MyAudio_master.MyAudio.Soundy.Sources.Soundies.Infrastructure.Interfaces;

namespace Sources.BoundedContexts.Bunkers.Infrastructure.Factories.Controllers
{
    public class BunkerPresenterFactory
    {
        private readonly IEntityRepository _entityRepository;
        private readonly ISoundyService _soundyService;

        public BunkerPresenterFactory(IEntityRepository entityRepository, ISoundyService soundyService)
        {
            _entityRepository = entityRepository ??
                                throw new ArgumentNullException(nameof(entityRepository));
            _soundyService = soundyService ?? throw new ArgumentNullException(nameof(soundyService));
        }

        public BunkerPresenter Create(IBunkerView bunkerView)
        {
            return new BunkerPresenter(_entityRepository, bunkerView, _soundyService);
        }
    }
}