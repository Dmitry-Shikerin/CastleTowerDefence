using Sources.Frameworks.GameServices.Volumes.Domain.Models.Implementation;

namespace Sources.BoundedContexts.Scenes.Domain
{
    public class MainMenuModel
    {
        public MainMenuModel(
            Volume musicVolume, 
            Volume soundsVolume)
        {
            MusicVolume = musicVolume;
            SoundsVolume = soundsVolume;
        }

        public Volume MusicVolume { get; }
        public Volume SoundsVolume { get; }
    }
}