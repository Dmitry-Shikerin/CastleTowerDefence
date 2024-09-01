using System;
using Sources.Frameworks.Domain.Interfaces.Entities;
using UnityEngine;

namespace Sources.Frameworks.GameServices.DailyRewards.Domain
{
    public class DailyReward : IEntity
    {
        public string Id { get; set; }
        public Type Type => GetType();
        public DateTime LastRewardTime { get; set; }
        public TimeSpan CurrentTime { get; set; }
        public DateTime TargetRewardTime { get; set; }
        public DateTime ServerTime { get; set; }
        public TimeSpan Delay { get; } = TimeSpan.FromSeconds(1);
        public bool IsAvailable => CurrentTime <= TimeSpan.Zero;
        public string TimerText => GetText();

        public void SetCurrentTime() => 
            CurrentTime = TargetRewardTime - ServerTime;

        public bool TrySetTargetRewardTime()
        {
            if (IsAvailable == false)
                return false;
            
            LastRewardTime = ServerTime;
            TargetRewardTime = LastRewardTime + TimeSpan.FromDays(1);
            Debug.Log($"SetTargetRewardTime: {TargetRewardTime}");
            return true;
        }
        
        private string GetText()
        {
            if (IsAvailable)
                return "00:00:00";

            string hours = CurrentTime.Hours < 10 ? "0" + CurrentTime.Hours : CurrentTime.Hours.ToString();
            string minutes = CurrentTime.Minutes < 10 ? "0" + CurrentTime.Minutes : CurrentTime.Minutes.ToString();
            string seconds = CurrentTime.Seconds < 10 ? "0" + CurrentTime.Seconds : CurrentTime.Seconds.ToString();
            
            return $"{hours}:{minutes}:{seconds}";
        }
    }
}