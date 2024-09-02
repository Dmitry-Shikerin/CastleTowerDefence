using System;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Domain;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Interfaces;
using Sources.Frameworks.GameServices.Loads.Domain.Constant;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces;
using UnityEngine;

namespace Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Implementation
{
    public class SaveVolumeButtonCommand : IButtonCommand
    {
        private readonly ILoadService _loadService;

        public SaveVolumeButtonCommand(ILoadService loadService)
        {
            _loadService = loadService ?? throw new ArgumentNullException(nameof(loadService));
        }

        public ButtonCommandId Id => ButtonCommandId.SaveVolume;
        
        public void Handle()
        {
            _loadService.Save(ModelId.SoundsVolume);
            _loadService.Save(ModelId.MusicVolume);
        }
    }
}