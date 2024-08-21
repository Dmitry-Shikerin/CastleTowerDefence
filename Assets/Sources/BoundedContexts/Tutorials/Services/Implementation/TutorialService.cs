using System;
using Doozy.Runtime.Signals;
using Sources.BoundedContexts.Tutorials.Domain;
using Sources.BoundedContexts.Tutorials.Services.Interfaces;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Constants;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces;

namespace Sources.BoundedContexts.Tutorials.Services.Implementation
{
    public class TutorialService : ITutorialService
    {
        private readonly ILoadService _loadService;
        
        private Tutorial _tutorial;
        private SignalStream _stream;
        private bool _simpleEnemyTutorialShowed;
        private bool _kamikazeEnemyTutorialShowed;
        private bool _bossEnemyTutorialShowed;

        public TutorialService(ILoadService loadService)
        {
            _loadService = loadService ?? throw new ArgumentNullException(nameof(loadService));
        }

        public void Initialize()
        {
            if (_tutorial.HasCompleted)
                return;
            
            _stream = SignalStream.Get(StreamConst.Gameplay, StreamConst.ShowTutorial);
            _stream.SendSignal(true);
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

        public void ShowSimpleEnemyTutorial()
        {
            if (_simpleEnemyTutorialShowed)
                return;
            
            _stream = SignalStream.Get(StreamConst.Gameplay, StreamConst.ShowSimpleEnemyTutorial);
            _stream.SendSignal(true);
            _simpleEnemyTutorialShowed = true;
        }

        public void ShowKamikazeEnemyTutorial()
        {
            if (_kamikazeEnemyTutorialShowed)
                return;
            
            _stream = SignalStream.Get(StreamConst.Gameplay, StreamConst.ShowKamikazeEnemyTutorial);
            _stream.SendSignal(true);
            _kamikazeEnemyTutorialShowed = true;
        }

        public void ShowBossEnemyTutorial()
        {
            if ( _bossEnemyTutorialShowed)
                return;
            
            _stream = SignalStream.Get(StreamConst.Gameplay, StreamConst.ShowBossEnemyTutorial);
            _stream.SendSignal(true);

            _bossEnemyTutorialShowed = true;
        }
    }
}