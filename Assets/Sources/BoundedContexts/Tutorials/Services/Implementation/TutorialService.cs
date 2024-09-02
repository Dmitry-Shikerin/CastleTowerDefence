using System;
using Doozy.Runtime.Signals;
using JetBrains.Annotations;
using Sources.BoundedContexts.Tutorials.Domain.Models;
using Sources.BoundedContexts.Tutorials.Services.Interfaces;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Constants;
using Sources.Frameworks.GameServices.Loads.Domain.Constant;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces;
using Sources.Frameworks.GameServices.Pauses.Services.Interfaces;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;

namespace Sources.BoundedContexts.Tutorials.Services.Implementation
{
    public class TutorialService : ITutorialService
    {
        private readonly IEntityRepository _entityRepository;
        private readonly ILoadService _loadService;
        private readonly IPauseService _pauseService;
        
        private Tutorial _tutorial;
        private SignalStream _stream;

        public TutorialService(
            IEntityRepository entityRepository,
            ILoadService loadService, 
            IPauseService pauseService)
        {
            _entityRepository = entityRepository ?? throw new ArgumentNullException(nameof(entityRepository));
            _loadService = loadService ?? throw new ArgumentNullException(nameof(loadService));
            _pauseService = pauseService ?? throw new ArgumentNullException(nameof(pauseService));
        }

        public void Initialize()
        {
            _tutorial = _entityRepository.Get<Tutorial>(ModelId.Tutorial);
            _stream = SignalStream.Get(StreamConst.Gameplay, StreamConst.ShowTutorial);
            
            if (_tutorial.HasCompleted)
                return;
            
            _stream.SendSignal(true);
            _pauseService.Pause();
        }

        public void Complete()
        {
            _tutorial.HasCompleted = true;
            _loadService.Save(_tutorial);
        }
    }
}