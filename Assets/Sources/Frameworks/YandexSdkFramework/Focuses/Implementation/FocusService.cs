using System;
using Sources.Frameworks.GameServices.Pauses.Services.Interfaces;
using Sources.Frameworks.YandexSdkFramework.Focuses.Interfaces;

namespace Sources.Frameworks.YandexSdkFramework.Focuses.Implementation
{
    public class FocusService : IFocusService
    {
        private readonly IPauseService _pauseService;

        public FocusService(IPauseService pauseService)
        {
            _pauseService = pauseService ?? throw new ArgumentNullException(nameof(pauseService));
        }
        
        public void Initialize()
        {
        }

        public void Destroy()
        {
        }

        private void OnInBackgroundChangeApp(bool inApp)
        {
            if (inApp == false)
            {
                _pauseService.Pause();
                _pauseService.PauseSound();

                return;
            }

            if (_pauseService.IsPaused)
                _pauseService.Continue();

            if (_pauseService.IsSoundPaused)
                _pauseService.ContinueSound();
        }

        private void OnInBackgroundChangeWeb(bool isBackground)
        {
            if (isBackground)
            {
                _pauseService.Pause();
                _pauseService.PauseSound();

                return;
            }

            if (_pauseService.IsPaused)
                _pauseService.Continue();

            if (_pauseService.IsSoundPaused)
                _pauseService.ContinueSound();
        }
    }
}