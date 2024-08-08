using System;
using MyAudios.Soundy.Editor.Presenters.Controllers;
using MyAudios.Soundy.Editor.SoundDataBases.Infrastructure.Factories;
using MyAudios.Soundy.Editor.SoundDataBases.Presentation.Views.Interfaces;
using MyAudios.Soundy.Editor.SoundyDataBases.Presentation.Views.Interfaces;
using MyAudios.Soundy.Editor.SoundySetting.Infrastructure.Factories;
using MyAudios.Soundy.Editor.SoundySetting.Presentation.Views.Interfaces;
using MyAudios.Soundy.Sources.DataBases.Domain.Data;
using MyAudios.Soundy.Sources.Settings.Domain.Configs;

namespace MyAudios.Soundy.Editor.SoundyDataBases.Controllers
{
    public class SoundyDataBasePresenter : IPresenter
    {
        private readonly SoundyDatabase _soundyDatabase;
        private readonly SoundySettings _soundySettings;
        private readonly ISoundyDataBaseView _view;
        private readonly SoundDataBaseViewFactory _soundDataBaseViewFactory;
        private readonly SoundySettingsViewFactory _soundySettingsViewFactory;

        public SoundyDataBasePresenter(
            SoundyDatabase soundyDatabase,
            SoundySettings soundySettings,
            ISoundyDataBaseView view,
            SoundDataBaseViewFactory soundDataBaseViewFactory,
            SoundySettingsViewFactory soundySettingsViewFactory)
        {
            _soundyDatabase = soundyDatabase ?? throw new ArgumentNullException(nameof(soundyDatabase));
            _soundySettings = soundySettings ?? throw new ArgumentNullException(nameof(soundySettings));
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _soundDataBaseViewFactory = soundDataBaseViewFactory ?? 
                                        throw new ArgumentNullException(nameof(soundDataBaseViewFactory));
            _soundySettingsViewFactory = soundySettingsViewFactory ?? throw new ArgumentNullException(nameof(soundySettingsViewFactory));
        }

        public void Initialize()
        {
            AddDataBasesButtons();
        }

        public void Dispose()
        {
            
        }

        private void CreateView(SoundDatabase soundDatabase)
        {
            _view.SettingsView?.Dispose();
            ISoundDataBaseView view = _soundDataBaseViewFactory.Create(soundDatabase, _soundyDatabase);
            view.SetSoundyDataBaseView(_view);
            _view.SetSoundDataBase(view);
        }

        private void AddDataBasesButtons()
        {
            foreach (SoundDatabase database in _soundyDatabase.SoundDatabases)
                _view.AddDataBaseButton(database.DatabaseName,() => CreateView(database));
        }

        public void CreateNewDataBase()
        {
            _soundyDatabase.CreateSoundDatabase("Default", true, true);
            _view.RefreshDataBasesButtons();
            AddDataBasesButtons();
        }

        public void RefreshDataBases() =>
            _soundyDatabase.RefreshDatabase();

        public void RenameButtons()
        {
            for (int i = 0; i < _view.DatabaseButtons.Count; i++)
                _view.DatabaseButtons[i].SetLabelText(_soundyDatabase.SoundDatabases[i].DatabaseName);
        }

        public void UpdateDataBase()
        {
            _view.ClearButtons();
            AddDataBasesButtons();
        }

        public void OpenSettings()
        {
            ISoundySettingsView view =_soundySettingsViewFactory.Create(_soundySettings);
            _view.AddSettings(view);
        }
    }
}