using System;
using Sources.Frameworks.UiFramework.Domain.Commands;
using Sources.Frameworks.UiFramework.InfrastructureInterfaces.Commands;
using Sources.InfrastructureInterfaces.Services.PauseServices;

namespace Sources.Frameworks.UiFramework.Infrastructure.Commands.Forms
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