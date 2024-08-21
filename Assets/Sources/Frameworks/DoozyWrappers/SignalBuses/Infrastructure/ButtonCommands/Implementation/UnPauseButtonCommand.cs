using System;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Domain;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Interfaces;
using Sources.Frameworks.GameServices.Pauses.Services.Interfaces;

namespace Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Implementation
{
    public class UnPauseButtonCommand : IButtonCommand
    {
        private readonly IPauseService _pauseService;

        public UnPauseButtonCommand(IPauseService pauseService)
        {
            _pauseService = pauseService ?? throw new ArgumentNullException(nameof(pauseService));
        }

        public ButtonCommandId Id => ButtonCommandId.UnPause;

        public void Handle()
        {
            if (_pauseService.IsPaused == false)
                return;
            
            _pauseService.Continue();
        }
    }
}