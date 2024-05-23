using System;
using Sources.Domain.Models.Data.Ids;
using Sources.Frameworks.UiFramework.Domain.Commands;
using Sources.Frameworks.UiFramework.InfrastructureInterfaces.Commands;
using Sources.InfrastructureInterfaces.Services.LoadServices;

namespace Sources.Frameworks.UiFramework.Infrastructure.Commands.Forms
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