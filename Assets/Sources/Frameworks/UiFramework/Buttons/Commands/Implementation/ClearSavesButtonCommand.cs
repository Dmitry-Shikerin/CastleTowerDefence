using System;
using Sources.Frameworks.UiFramework.Domain.Commands;
using Sources.Frameworks.UiFramework.InfrastructureInterfaces.Commands.Buttons;
using Sources.Frameworks.UiFramework.Presentation.Buttons;
using Sources.Frameworks.UiFramework.PresentationsInterfaces.Buttons;
using Sources.InfrastructureInterfaces.Services.LoadServices;

namespace Sources.Frameworks.UiFramework.Infrastructure.Commands.Buttons
{
    public class ClearSavesButtonCommand : IButtonCommand
    {
        private readonly ILoadService _loadService;

        public ClearSavesButtonCommand(ILoadService loadService)
        {
            _loadService = loadService ?? throw new ArgumentNullException(nameof(loadService));
        }

        public ButtonCommandId Id => ButtonCommandId.ClearSaves;
        
        public void Handle(IUiButton uiButton)
        {
            _loadService.ClearAll();
        }
    }
}