using System;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ViewCommands.Interfaces;
using Sources.Frameworks.UiFramework.Domain.Commands;
using Sources.InfrastructureInterfaces.Services.LoadServices;

namespace Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ViewCommands.Implementation
{
    public class ClearSavesCommand : IViewCommand
    {
        private readonly ILoadService _loadService;

        public ClearSavesCommand(ILoadService loadService)
        {
            _loadService = loadService ?? throw new ArgumentNullException(nameof(loadService));
        }

        public FormCommandId Id => FormCommandId.ClearSaves;
        
        public void Handle()
        {
            _loadService.ClearAll();
        }
    }
}