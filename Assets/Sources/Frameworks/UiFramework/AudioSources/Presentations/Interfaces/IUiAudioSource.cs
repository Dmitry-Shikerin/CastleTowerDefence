using System;
using Cysharp.Threading.Tasks;
using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.Views;
using Sources.Frameworks.UiFramework.AudioSources.Domain.Groups;
using Sources.Frameworks.UiFramework.AudioSources.Presentations.Implementation.Types;
using UnityEngine;

namespace Sources.Frameworks.UiFramework.AudioSources.Presentations.Interfaces
{
    public interface IUiAudioSource : IView
    {
        AudioSourceId AudioSourceId { get; }
        bool IsPlaying { get; }

        UniTask PlayAsync(Action callback = null, AudioGroup audioGroup = null);
        void StopPlayAsync();
        void Play();
        IUiAudioSource SetVolume(float volume);
        IUiAudioSource SetClip(AudioClip clip);
        void Pause();
        void UnPause();
    }
}