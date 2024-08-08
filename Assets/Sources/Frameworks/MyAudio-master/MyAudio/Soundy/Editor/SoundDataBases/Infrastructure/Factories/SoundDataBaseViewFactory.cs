using MyAudios.Soundy.Editor.SoundDataBases.Controllers;
using MyAudios.Soundy.Editor.SoundDataBases.Presentation.Views.Implementation;
using MyAudios.Soundy.Editor.SoundDataBases.Presentation.Views.Interfaces;
using MyAudios.Soundy.Editor.SoundGroups.Infrastructure.Factories;
using MyAudios.Soundy.Sources.DataBases.Domain.Data;

namespace MyAudios.Soundy.Editor.SoundDataBases.Infrastructure.Factories
{
    public class SoundDataBaseViewFactory
    {
        public ISoundDataBaseView Create(SoundDatabase soundDatabase, SoundyDatabase soundyDatabase)
        {
            SoundGroupViewFactory soundGroupViewFactory = new SoundGroupViewFactory();
            
            SoundDataBaseView view = new SoundDataBaseView();
            SoundDataBasePresenter presenter = new SoundDataBasePresenter(
                soundDatabase, soundyDatabase, view,  soundGroupViewFactory);
            view.Construct(presenter);
            
            return view;
        }
    }
}