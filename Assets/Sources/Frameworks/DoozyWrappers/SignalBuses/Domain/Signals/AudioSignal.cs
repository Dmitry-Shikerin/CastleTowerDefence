using Sources.Frameworks.UiFramework.AudioSources.Presentations.Implementation.Types;

namespace Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Signals
{
    public struct AudioSignal
    {
        public AudioSignal(AudioClipId audioClipId)
        {
            AudioClipId = audioClipId;
        }
        
        public AudioClipId AudioClipId { get; }
    }
}