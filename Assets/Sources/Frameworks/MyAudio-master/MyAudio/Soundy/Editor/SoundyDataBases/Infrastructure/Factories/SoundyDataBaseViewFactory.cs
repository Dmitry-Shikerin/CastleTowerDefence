using MyAudios.Soundy.Editor.SoundDataBases.Infrastructure.Factories;
using MyAudios.Soundy.Editor.SoundyDataBases.Controllers;
using MyAudios.Soundy.Editor.SoundyDataBases.Presentation.Views.Interfaces;
using MyAudios.Soundy.Editor.SoundyDataBases.Views.Implementation;
using MyAudios.Soundy.Editor.SoundySetting.Infrastructure.Factories;
using MyAudios.Soundy.Sources.DataBases.Domain.Data;
using MyAudios.Soundy.Sources.Settings.Domain.Configs;

namespace MyAudios.Soundy.Editor.SoundyDataBases.Infrastructure.Factories
{
    public class SoundyDataBaseViewFactory
    {
        public ISoundyDataBaseView Create(SoundyDatabase soundyDatabase, SoundySettings soundySettings)
        {
            SoundySettingsViewFactory soundySettingsViewFactory = new SoundySettingsViewFactory();
            SoundDataBaseViewFactory soundDataBaseViewFactory = new SoundDataBaseViewFactory();
            
            SoundyDataBaseView view = new SoundyDataBaseView();
            SoundyDataBasePresenter presenter = new SoundyDataBasePresenter(
                soundyDatabase, soundySettings, view, soundDataBaseViewFactory, soundySettingsViewFactory);
            view.Construct(presenter);
            
            return view;
        }
    }
}