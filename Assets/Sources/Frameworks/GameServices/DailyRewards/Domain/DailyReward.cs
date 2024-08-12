using System;
using Sources.Frameworks.Domain.Interfaces.Entities;

namespace Sources.Frameworks.GameServices.DailyRewards.Domain
{
    public class DailyReward : IEntity
    {
        public DailyReward(string id)
        {
            Id = id;
        }

        public string Id { get; }
        public Type Type => GetType();
        public DateTime LastRewardTime { get; set; }
        public TimeSpan CurrentTime { get; set; }
        public DateTime TargetRewardTime { get; set; }
        public DateTime ServerTime { get; set; }
        public TimeSpan Delay { get; } = TimeSpan.FromSeconds(1);

        public void SetCurrentTime()
        {
            CurrentTime = LastRewardTime - ServerTime;
        }
    }
}