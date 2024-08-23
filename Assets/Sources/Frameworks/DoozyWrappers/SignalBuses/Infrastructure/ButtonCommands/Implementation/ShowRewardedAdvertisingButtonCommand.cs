using System;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Domain;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Interfaces;
using Sources.Frameworks.YandexSdkFramework.Advertisings.Services.Interfaces;

namespace Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Implementation
{
    public class ShowRewardedAdvertisingButtonCommand : IButtonCommand
    {
        private readonly IVideoAdService _videoAdService;

        public ShowRewardedAdvertisingButtonCommand(IVideoAdService videoAdService)
        {
            _videoAdService = videoAdService ?? throw new ArgumentNullException(nameof(videoAdService));
        }

        public ButtonCommandId Id => ButtonCommandId.ShowRewardedAdvertising;

        public void Handle() => 
            _videoAdService.ShowVideo(null);
    }
}