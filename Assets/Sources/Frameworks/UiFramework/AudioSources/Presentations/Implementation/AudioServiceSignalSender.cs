using Doozy.Runtime.Signals;
using Sources.Frameworks.GameServices.DoozySignalBuses.Domain.Constants;
using Sources.Frameworks.UiFramework.AudioSources.Domain.Signals;
using Sources.Frameworks.UiFramework.AudioSources.Presentations.Implementation.Types;
using UnityEngine;

namespace Sources.Frameworks.UiFramework.AudioSources.Presentations.Implementation
{
    public abstract class AudioServiceSignalSender : MonoBehaviour
    {
        private SignalStream _stream;
        
        private void Awake()
        {
            _stream = SignalStream.Get(StreamConst.SoundCommand, StreamConst.Play);
            OnAfterAwake();
        }

        private void OnEnable() =>
            OnAfterEnable();

        private void OnDisable() =>
            OnAfterDisable();

        protected virtual void OnAfterAwake()
        {
        }

        protected virtual void OnAfterDisable()
        {
        }

        protected virtual void OnAfterEnable()
        {
        }

        protected void SendSignal(AudioClipId audioClipId) =>
            _stream.SendSignal(new AudioSignal(audioClipId));
    }
}