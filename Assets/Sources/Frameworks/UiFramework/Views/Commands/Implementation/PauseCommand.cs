using System;
using Sources.Frameworks.UiFramework.Domain.Commands;
using Sources.Frameworks.UiFramework.InfrastructureInterfaces.Commands;
using Sources.InfrastructureInterfaces.Services.PauseServices;

namespace Sources.Frameworks.UiFramework.Infrastructure.Commands.Forms
{
    public class PauseCommand : IViewCommand
    {
        private readonly IPauseService _pauseService;

        public PauseCommand(IPauseService pauseService)
        {
            _pauseService = pauseService ?? throw new ArgumentNullException(nameof(pauseService));
        }

        public FormCommandId Id => FormCommandId.Pause;
        
        public void Handle() =>
            _pauseService.Pause();
    }
}