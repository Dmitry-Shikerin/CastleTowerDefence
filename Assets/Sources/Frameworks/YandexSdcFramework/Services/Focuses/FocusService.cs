using System;
using Agava.WebUtility;
using Sources.Frameworks.YandexSdcFramework.ServicesInterfaces.Focuses;
using Sources.InfrastructureInterfaces.Services.PauseServices;
using UnityEngine;

namespace Sources.Frameworks.YandexSdcFramework.Services.Focuses
{
    public class FocusService : IFocusService
    {
        private readonly IPauseService _pauseService;

        public FocusService(IPauseService pauseService)
        {
            _pauseService = pauseService ?? throw new ArgumentNullException(nameof(pauseService));
        }
        
        public void Enable()
        {
            if (WebApplication.IsRunningOnWebGL == false)
                return;

            OnInBackgroundChangeWeb(WebApplication.InBackground);
            OnInBackgroundChangeApp(Application.isFocused);
            
            Application.focusChanged += OnInBackgroundChangeApp;
            WebApplication.InBackgroundChangeEvent += OnInBackgroundChangeWeb;
        }

        public void Disable()
        {
            if (WebApplication.IsRunningOnWebGL == false)
                return;
            
            Application.focusChanged -= OnInBackgroundChangeApp;
            WebApplication.InBackgroundChangeEvent -= OnInBackgroundChangeWeb;
        }

        private void OnInBackgroundChangeApp(bool inApp)
        {
            // Debug.Log($"FocusService: {inApp}");
            //
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
            // Debug.Log($"FocusService: {isBackground}");
            //
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