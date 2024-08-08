using Doozy.Engine.Soundy;
using MyAudios.Soundy.Editor.AudioDatas.Controllers;
using MyAudios.Soundy.Editor.AudioDatas.Presentation.View.Implementation;
using MyAudios.Soundy.Editor.AudioDatas.Presentation.View.Interfaces;
using MyAudios.Soundy.Sources.DataBases.Domain.Data;

namespace MyAudios.Soundy.Editor.AudioDatas.Infrastructure.Factories
{
    public class AudioDataViewFactory
    {
        public IAudioDataView Create(AudioData audioData, SoundGroupData soundGroupData)
        {
            AudioDataView audioDataView = new AudioDataView();
            AudioDataPresenter presenter = new AudioDataPresenter(audioData, soundGroupData, audioDataView);
            audioDataView.Construct(presenter);
            
            return audioDataView;
        }
    }
}