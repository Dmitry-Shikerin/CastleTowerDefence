using System;
using Sources.BoundedContexts.Tutorials.Services.Interfaces;
using Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Commands.Interfaces;
using Sources.Frameworks.UiFramework.Domain.Commands;

namespace Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Commands.Implementation
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