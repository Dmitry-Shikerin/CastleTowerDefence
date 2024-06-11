using System;
using Doozy.Runtime.Signals;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Controllers.Interfaces;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Constants;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Signals;
using Sources.Frameworks.UiFramework.AudioSources.Infrastructure.Services.AudioService.Interfaces;

namespace Sources.Frameworks.DoozyWrappers.SignalBuses.Controllers.Implementation
{
    public class AudioServiceSignalController : ISignalController
    {
        private readonly IAudioService _audioService;

        private SignalReceiver _signalReceiver;
        private SignalStream _signalStream;

        public AudioServiceSignalController(IAudioService audioService)
        {
            _audioService = audioService ?? throw new ArgumentNullException(nameof(audioService));
        }

        public void Initialize()
        {
            _signalReceiver =
                new SignalReceiver()
                    .SetOnSignalCallback(Handle);
            _signalStream = SignalStream
                .Get(StreamConst.SoundCommand, StreamConst.Play)
                .ConnectReceiver(_signalReceiver);
        }

        public void Destroy() =>
            _signalStream.DisconnectReceiver(_signalReceiver);

        private void Handle(Signal signal)
        {
            if (signal.TryGetValue(out AudioSignal value) == false)
                throw new InvalidOperationException("Signal valueAsObject is not AudioSignal");

            _audioService.Play(value.AudioClipId);
        }
    }
}