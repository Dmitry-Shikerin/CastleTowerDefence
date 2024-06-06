using System;
using Sources.Frameworks.UiFramework.ButtonProviders.Domain;
using Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Commands.Interfaces;
using Sources.Frameworks.UiFramework.Domain.Commands;
using Sources.InfrastructureInterfaces.Services.LoadServices;

namespace Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Commands.Implementation
{
    public class ClearSavesButtonCommand : IButtonCommand
    {
        private readonly ILoadService _loadService;

        public ClearSavesButtonCommand(ILoadService loadService)
        {
            _loadService = loadService ?? throw new ArgumentNullException(nameof(loadService));
        }

        public ButtonCommandId Id => ButtonCommandId.ClearSaves;

        public void Handle() =>
            _loadService.ClearAll();
    }
}