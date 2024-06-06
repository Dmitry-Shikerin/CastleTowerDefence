using System;
using Doozy.Runtime.Signals;
using Sources.Frameworks.GameServices.DoozySignalBuses.Controllers.Interfaces;
using Sources.Frameworks.UiFramework.AudioSources.Domain.Signals;
using Sources.Frameworks.UiFramework.AudioSources.Infrastructure.Services.AudioService.Interfaces;
using Sources.Frameworks.UiFramework.Domain.Commands;

namespace Sources.BoundedContexts.SignalCollectors.Controllers
{
    public class AudioServiceSignalController : ISignalController
    {
        private readonly IAudioService _audioService;

        private SignalReceiver _signalReceiver;

        public AudioServiceSignalController(IAudioService audioService)
        {
            _audioService = audioService ?? throw new ArgumentNullException(nameof(audioService));
        }

        public void Initialize()
        {
            _signalReceiver = 
                new SignalReceiver()
                    .SetOnSignalCallback(Handle);
            SignalStream
                .Get("SoundCommand", "Play")
                .ConnectReceiver(_signalReceiver);

        }

        public void Destroy()
        {
            SignalStream
                .Get("SoundCommand", "Play")
                .DisconnectReceiver(_signalReceiver);
        }
        
        private void Handle(Signal signal)
        {
            if (signal.TryGetValue(out AudioSignal value) == false)
                throw new InvalidOperationException("Signal valueAsObject is not AudioSignal");

            _audioService.Play(value.AudioClipId);
        }
    }
}