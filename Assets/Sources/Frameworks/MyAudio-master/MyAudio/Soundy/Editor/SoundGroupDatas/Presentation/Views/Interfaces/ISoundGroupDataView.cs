using System.Collections.Generic;
using Doozy.Engine.Soundy;
using MyAudios.Soundy.Editor.AudioDatas.Presentation.View.Interfaces;
using MyAudios.Soundy.Editor.Views;
using UnityEngine;

namespace MyAudios.Soundy.Editor.SoundGroupDatas.Presentation.Views.Interfaces
{
    public interface ISoundGroupDataView : IView
    {
        IReadOnlyList<IAudioDataView> AudioDataViews { get; }
        
        void AddAudioData(IAudioDataView audioDataView);
        void SetIsOnButtonTab(SoundGroupData.PlayMode playMode);
        void SetVolume(Vector2 volume, Vector2 minMaxVolume);
        void SetPitch(Vector2 pitch, Vector2 minMaxPitch);
        void SetSpatialBlend(float spatialBlend, Vector2 minMaxSpatialBlend);
        public void SetSoundName(string name);
        void StopAllAudioData();
        void SetLoop(bool loop);
    }
}