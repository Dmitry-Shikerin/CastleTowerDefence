

using MyAudios.Soundy.Editor.SoundGroupDatas.Presentation.Views.Interfaces;
using MyAudios.Soundy.Editor.Views;
using UnityEngine;

namespace MyAudios.Soundy.Editor.AudioDatas.Presentation.View.Interfaces
{
    public interface IAudioDataView : IView
    {
        void StopPlaySound();
        void SetSliderValue(float value);        
        void SetSliderValue(Vector2 minMaxValue, float value);
        void SetStopIcon();
        void SetPlayIcon();
        void SetAudioClip(AudioClip audioClip);
        void StopAllSounds();
        void SetSoundGroupData(ISoundGroupDataView soundGroupDataView);
    }
}