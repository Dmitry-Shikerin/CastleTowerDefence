using System.Collections.Generic;
using Sources.Frameworks.GameServices.DailyRewards.Domain;
using Sources.Frameworks.GameServices.Volumes.Domain.Models.Implementation;
using Sources.Frameworks.MyGameCreator.Achivements.Domain.Models;

namespace Sources.BoundedContexts.Scenes.Domain
{
    public class MainMenuModel
    {
        public MainMenuModel(
            Volume musicVolume, 
            Volume soundsVolume,
            DailyReward dailyReward,
            List<Achievement> achievements) 
        {
            MusicVolume = musicVolume;
            SoundsVolume = soundsVolume;
            DailyReward = dailyReward;
            Achievements = achievements;
        }

        public Volume MusicVolume { get; }
        public Volume SoundsVolume { get; }
        public DailyReward DailyReward { get; }
        public List<Achievement> Achievements { get; }
    }
}