using System;
using Sources.Frameworks.UiFramework.Domain.Commands;
using Sources.Frameworks.UiFramework.InfrastructureInterfaces.Commands.Buttons;
using Sources.Frameworks.UiFramework.Presentation.Buttons;
using Sources.Frameworks.UiFramework.PresentationsInterfaces.Buttons;
using Sources.InfrastructureInterfaces.Services.PauseServices;

namespace Sources.Frameworks.UiFramework.Infrastructure.Commands.Buttons
{
    public class UnPauseButtonCommand : IButtonCommand
    {
        private readonly IPauseService _pauseService;

        public UnPauseButtonCommand(IPauseService pauseService)
        {
            _pauseService = pauseService ?? throw new ArgumentNullException(nameof(pauseService));
        }

        public ButtonCommandId Id => ButtonCommandId.UnPause;
        
        public void Handle(IUiButton uiButton)
        {
            _pauseService.Continue();
        }
    }
}