using Doozy.Engine.Soundy;
using MyAudios.Soundy.Editor.AudioDatas.Infrastructure.Factories;
using MyAudios.Soundy.Editor.SoundGroupDatas.Controllers;
using MyAudios.Soundy.Editor.SoundGroupDatas.Presentation.Views.Implementation;
using MyAudios.Soundy.Editor.SoundGroupDatas.Presentation.Views.Interfaces;
using MyAudios.Soundy.Sources.DataBases.Domain.Data;

namespace MyAudios.Soundy.Editor.SoundGroupDatas.Infrastructure.Factories
{
    public class SoundGroupDataViewFactory
    {
        public ISoundGroupDataView Create(SoundGroupData soundGroupData, SoundDatabase soundDatabase)
        {
            AudioDataViewFactory audioDataViewFactory = new AudioDataViewFactory();
            
            SoundGroupDataView view = new SoundGroupDataView();
            SoundGroupDataPresenter presenter = new SoundGroupDataPresenter(
                soundGroupData, soundDatabase, view, audioDataViewFactory);
            view.Construct(presenter);
            
            return view;
        }
    }
}