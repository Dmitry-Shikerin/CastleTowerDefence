using System;
using Doozy.Runtime.Signals;
using Sources.BoundedContexts.Tutorials.Domain;
using Sources.BoundedContexts.Tutorials.Services.Interfaces;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Constants;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces;
using Sources.Frameworks.UiFramework.ServicesInterfaces.Forms;

namespace Sources.BoundedContexts.Tutorials.Services.Implementation
{
    public class TutorialService : ITutorialService
    {
        private readonly ILoadService _loadService;
        
        private Tutorial _tutorial;
        private SignalStream _stream;

        public TutorialService(ILoadService loadService)
        {
            _loadService = loadService ?? throw new ArgumentNullException(nameof(loadService));
        }

        public void Initialize()
        {
            _stream = SignalStream.Get(StreamConst.Gameplay, StreamConst.ShowTutorial);
            
            if (_tutorial.HasCompleted)
                return;
            
            _stream.SendSignal(true);

            // if (_savedLevel.SavedLevelId != ModelId.Gameplay)
            //     return;
            //
            // _formService.Show(FormId.GreetingTutorial);
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