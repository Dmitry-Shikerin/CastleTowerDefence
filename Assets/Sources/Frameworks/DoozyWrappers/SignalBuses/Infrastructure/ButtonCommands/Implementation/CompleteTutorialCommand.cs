using System;
using Sources.BoundedContexts.Tutorials.Services.Interfaces;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Domain;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Interfaces;

namespace Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Implementation
{
    public class CompleteTutorialCommand : IButtonCommand
    {
        private readonly ITutorialService _tutorialService;

        public CompleteTutorialCommand(ITutorialService tutorialService)
        {
            _tutorialService = tutorialService ?? throw new ArgumentNullException(nameof(tutorialService));
        }

        public ButtonCommandId Id => ButtonCommandId.CompleteTutorial;

        public void Handle() =>
            _tutorialService.Complete();
    }
}