using System;
using Sources.Frameworks.GameServices.Pauses.Services.Interfaces;
using Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Commands.Interfaces;
using Sources.Frameworks.UiFramework.Domain.Commands;

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

        public void Handle() =>
            _pauseService.Continue();
    }
}