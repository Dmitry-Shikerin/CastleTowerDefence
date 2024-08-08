using System.Collections.Generic;
using Doozy.Editor.EditorUI.Components;
using MyAudios.Soundy.Editor.SoundDataBases.Presentation.Views.Interfaces;
using MyAudios.Soundy.Editor.SoundySetting.Presentation.Views.Interfaces;
using MyAudios.Soundy.Editor.Views;
using UnityEngine.Events;

namespace MyAudios.Soundy.Editor.SoundyDataBases.Presentation.Views.Interfaces
{
    public interface ISoundyDataBaseView : IView
    {
        public IReadOnlyList<FluidToggleButtonTab> DatabaseButtons { get; }
        public ISoundySettingsView SettingsView { get; }
        public ISoundDataBaseView SoundDataBaseView { get; }
        
        void AddDataBaseButton(string name, UnityAction callback);
        void SetSoundDataBase(ISoundDataBaseView dataBaseView);
        void RefreshDataBasesButtons();
        void RenameButtons();
        void UpdateDataBase();
        void ClearButtons();
        void AddSettings(ISoundySettingsView soundySettingsView);
    }
}