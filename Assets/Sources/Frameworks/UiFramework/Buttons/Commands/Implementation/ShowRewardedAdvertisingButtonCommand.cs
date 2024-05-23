using System;
using Sources.Frameworks.UiFramework.Domain.Commands;
using Sources.Frameworks.UiFramework.InfrastructureInterfaces.Commands.Buttons;
using Sources.Frameworks.UiFramework.Presentation.Buttons;
using Sources.Frameworks.UiFramework.PresentationsInterfaces.Buttons;
using Sources.Frameworks.YandexSdcFramework.ServicesInterfaces.AdverticingServices;

namespace Sources.Frameworks.UiFramework.Infrastructure.Commands.Buttons
{
    //TODO возможно это не команда а стратегия
    public class ShowRewardedAdvertisingButtonCommand : IButtonCommand
    {
        private readonly IVideoAdService _videoAdService;

        public ShowRewardedAdvertisingButtonCommand(IVideoAdService videoAdService)
        {
            _videoAdService = videoAdService ?? throw new ArgumentNullException(nameof(videoAdService));
        }

        public ButtonCommandId Id => ButtonCommandId.ShowRewardedAdvertising;
        
        public void Handle(IUiButton uiButton)
        {
            uiButton.Disable();
            _videoAdService.ShowVideo(uiButton.Enable);
        }
    }
}