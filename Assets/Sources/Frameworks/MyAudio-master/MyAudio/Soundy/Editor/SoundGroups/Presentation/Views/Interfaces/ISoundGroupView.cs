using System.Numerics;
using MyAudios.Soundy.Editor.SoundDataBases.Presentation.Views.Interfaces;
using MyAudios.Soundy.Editor.Views;

namespace MyAudios.Soundy.Editor.SoundGroups.Presentation.Views.Interfaces
{
    public interface ISoundGroupView : IView
    {
        void SetPlayIcon();
        void SetStopIcon();
        void SetSliderValue(float audioSourceTime);
        void SetSliderMaxValue(float maxValue);
        void StopAllAudioGroup();
        void SetDataBase(ISoundDataBaseView soundDataBaseView);
        void StopPlaySound();
        void SetSoundGroupName(string soundGroupName);
        void StopAllAudioData();
    }
}