using Sources.Frameworks.GameServices.DailyRewards.Domain;
using Sources.Frameworks.GameServices.Volumes.Domain.Models.Implementation;

namespace Sources.BoundedContexts.Scenes.Domain
{
    public class MainMenuModel
    {
        public MainMenuModel(
            Volume musicVolume, 
            Volume soundsVolume,
            DailyReward dailyReward)
        {
            MusicVolume = musicVolume;
            SoundsVolume = soundsVolume;
            DailyReward = dailyReward;
        }

        public Volume MusicVolume { get; }
        public Volume SoundsVolume { get; }
        public DailyReward DailyReward { get; }
    }
}