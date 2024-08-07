using MyAudios.Soundy.Editor.SoundGroups.Presentation.Views.Interfaces;
using MyAudios.Soundy.Editor.SoundyDataBases.Presentation.Views.Interfaces;
using MyAudios.Soundy.Editor.Views;
using UnityEngine.Audio;

namespace MyAudios.Soundy.Editor.SoundDataBases.Presentation.Views.Interfaces
{
    public interface ISoundDataBaseView : IView
    {
        void AddSoundGroup(ISoundGroupView soundGroupView);
        public void RemoveSoundGroup(ISoundGroupView soundGroupView);
        void StopAllSoundGroup();
        void SetName(string name);
        void SetAudioMixerGroup(AudioMixerGroup audioMixerGroup);
        void RenameDataBaseButtons();
        void SetSoundyDataBaseView(ISoundyDataBaseView view);
    }
}