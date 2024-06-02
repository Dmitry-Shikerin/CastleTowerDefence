using System;
using Sources.Frameworks.UiFramework.ButtonProviders.Domain;
using Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Commands.Interfaces;
using Sources.Frameworks.UiFramework.Domain.Commands;
using Sources.Frameworks.YandexSdcFramework.ServicesInterfaces.AdverticingServices;
using Sources.Frameworks.YandexSdkFramework.Advertisings.Services.Interfaces;

namespace Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Commands.Implementation
{
    public class ShowRewardedAdvertisingButtonCommand : IButtonCommand
    {
        private readonly IVideoAdService _videoAdService;

        public ShowRewardedAdvertisingButtonCommand(IVideoAdService videoAdService)
        {
            _videoAdService = videoAdService ?? throw new ArgumentNullException(nameof(videoAdService));
        }

        public ButtonCommandId Id => ButtonCommandId.ShowRewardedAdvertising;

        public void Handle()
        {
            // uiButton.Disable();
            _videoAdService.ShowVideo(null);
        }
    }
}