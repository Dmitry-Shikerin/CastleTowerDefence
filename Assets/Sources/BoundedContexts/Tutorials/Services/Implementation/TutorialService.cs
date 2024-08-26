using System;
using Doozy.Runtime.Signals;
using Sources.BoundedContexts.Tutorials.Domain.Models;
using Sources.BoundedContexts.Tutorials.Services.Interfaces;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Constants;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces;
using Sources.Frameworks.GameServices.Pauses.Services.Interfaces;

namespace Sources.BoundedContexts.Tutorials.Services.Implementation
{
    public class TutorialService : ITutorialService
    {
        private readonly ILoadService _loadService;
        private readonly IPauseService _pauseService;
        
        private Tutorial _tutorial;
        private SignalStream _stream;

        public TutorialService(ILoadService loadService, IPauseService pauseService)
        {
            _loadService = loadService ?? throw new ArgumentNullException(nameof(loadService));
            _pauseService = pauseService ?? throw new ArgumentNullException(nameof(pauseService));
        }

        public void Initialize()
        {
            _stream = SignalStream.Get(StreamConst.Gameplay, StreamConst.ShowTutorial);
            
            if (_tutorial.HasCompleted)
                return;
            
            _stream.SendSignal(true);
            _pauseService.Pause();
        }

        public void Construct(Tutorial tutorial)
        {
            _tutorial = tutorial ?? throw new ArgumentNullException(nameof(tutorial));
        }

        public void Complete()
        {
            _tutorial.HasCompleted = true;
            _loadService.Save(_tutorial);
        }
    }
}