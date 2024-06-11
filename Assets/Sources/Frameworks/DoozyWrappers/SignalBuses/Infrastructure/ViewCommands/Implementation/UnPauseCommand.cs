using System;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ViewCommands.Interfaces;
using Sources.Frameworks.GameServices.Pauses.Services.Interfaces;
using Sources.Frameworks.UiFramework.Domain.Commands;

namespace Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ViewCommands.Implementation
{
    public class UnPauseCommand : IViewCommand
    {
        private readonly IPauseService _pauseService;

        public UnPauseCommand(IPauseService pauseService)
        {
            _pauseService = pauseService ?? throw new ArgumentNullException(nameof(pauseService));
        }

        public FormCommandId Id => FormCommandId.UnPause;
        
        public void Handle() =>
            _pauseService.Continue();
    }
}