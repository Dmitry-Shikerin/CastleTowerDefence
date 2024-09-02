using System.Collections.Generic;
using Sources.BoundedContexts.HealthBoosters.Domain;
using Sources.Frameworks.GameServices.DailyRewards.Domain;
using Sources.Frameworks.GameServices.Volumes.Domain.Models.Implementation;
using Sources.Frameworks.MyGameCreator.Achievements.Domain.Models;

namespace Sources.BoundedContexts.Scenes.Domain
{
    public class MainMenuModel
    {
        public MainMenuModel(
            Volume musicVolume, 
            Volume soundsVolume,
            DailyReward dailyReward,
            List<Achievement> achievements,
            HealthBooster healthBooster) 
        {
            MusicVolume = musicVolume;
            SoundsVolume = soundsVolume;
            DailyReward = dailyReward;
            Achievements = achievements;
            HealthBooster = healthBooster;
        }

        public Volume MusicVolume { get; }
        public Volume SoundsVolume { get; }
        public DailyReward DailyReward { get; }
        public List<Achievement> Achievements { get; }
        public HealthBooster HealthBooster { get; }
    }
}