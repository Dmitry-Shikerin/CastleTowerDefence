using System;
using Sources.BoundedContexts.Tutorials.Domain;
using Sources.BoundedContexts.Tutorials.Services.Interfaces;
using Sources.Frameworks.UiFramework.ServicesInterfaces.Forms;
using Sources.InfrastructureInterfaces.Services.LoadServices;

namespace Sources.Infrastructure.Services.Tutorials
{
    public class TutorialService : ITutorialService
    {
        private readonly IFormService _formService;
        private readonly ILoadService _loadService;
        private Tutorial _tutorial;

        public TutorialService(
            IFormService formService,
            ILoadService loadService)
        {
            _formService = formService ?? throw new ArgumentNullException(nameof(formService));
            _loadService = loadService ?? throw new ArgumentNullException(nameof(loadService));
        }

        public void Enable()
        {
            if (_tutorial.HasCompleted)
                return;

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