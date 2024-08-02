using System;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Domain;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Interfaces;
using Sources.Frameworks.GameServices.Pauses.Services.Interfaces;

namespace Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Implementation
{
    public class PauseButtonCommand : IButtonCommand
    {
        private readonly IPauseService _pauseService;

        public PauseButtonCommand(IPauseService pauseService)
        {
            _pauseService = pauseService ?? throw new ArgumentNullException(nameof(pauseService));
        }

        public ButtonCommandId Id => ButtonCommandId.Pause;

        public void Handle() =>
            _pauseService.Pause();
    }
}