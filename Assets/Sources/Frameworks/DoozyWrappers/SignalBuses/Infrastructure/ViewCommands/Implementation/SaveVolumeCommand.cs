using System;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ViewCommands.Interfaces;
using Sources.Frameworks.UiFramework.Domain.Commands;
using Sources.InfrastructureInterfaces.Services.LoadServices;

namespace Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ViewCommands.Implementation
{
    public class SaveVolumeCommand : IViewCommand
    {
        private readonly ILoadService _loadService;

        public SaveVolumeCommand(ILoadService loadService)
        {
            _loadService = loadService ?? throw new ArgumentNullException(nameof(loadService));
        }

        public FormCommandId Id => FormCommandId.SaveVolume;
        
        public void Handle()
        {
            _loadService.Save(ModelId.Volume);
        }
    }
}