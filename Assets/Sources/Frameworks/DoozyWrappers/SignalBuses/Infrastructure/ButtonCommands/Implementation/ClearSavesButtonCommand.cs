using System;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Domain;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Interfaces;
using Sources.InfrastructureInterfaces.Services.LoadServices;

namespace Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Implementation
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