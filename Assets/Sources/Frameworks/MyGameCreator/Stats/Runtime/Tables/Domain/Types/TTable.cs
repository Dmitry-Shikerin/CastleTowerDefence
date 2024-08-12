using System;
using UnityEngine;

namespace Sources.Frameworks.MyGameCreator.Stats.Tables.Domain.Types
{
    [Serializable]
    public abstract class TTable
    {
        public virtual int MinLevel => 1;
        public virtual int MaxLevel => 99;


        public int GetLevelForCumulativeExperience(int cumulative) =>
            LevelFromCumulative(cumulative);

        public int GetCumulativeExperienceForLevel(int level)
        {
            level = Mathf.Clamp(level, this.MinLevel, this.MaxLevel + 1);
            
            return CumulativeFromLevel(level);
        }

        public int GetLevelExperienceForLevel(int level)
        {
            int currLevel = Mathf.Clamp(level + 0, this.MinLevel, this.MaxLevel + 1);
            int nextLevel = Mathf.Clamp(level + 1, this.MinLevel, this.MaxLevel + 1);

            int cumulativeCurrent = CumulativeFromLevel(currLevel);
            int cumulativeNext = CumulativeFromLevel(nextLevel);

            return cumulativeNext - cumulativeCurrent;
        }

        public int GetLevelExperienceAtCurrentLevel(int cumulative)
        {
            int currentLevel = GetLevelForCumulativeExperience(cumulative);
            return cumulative - CumulativeFromLevel(currentLevel);
        }

        public int GetLevelExperienceToNextLevel(int cumulative)
        {
            int nextLevel = GetNextLevel(cumulative);
            return CumulativeFromLevel(nextLevel) - cumulative;
        }

        public float GetRatioAtCurrentLevel(int cumulative)
        {
            int experienceFrom = GetLevelExperienceAtCurrentLevel(cumulative);
            int experienceTo = GetLevelExperienceToNextLevel(cumulative);
            return (float)experienceFrom / (experienceFrom + experienceTo);
        }

        public float GetRatioForNextLevel(int cumulative) =>
            1f - GetRatioAtCurrentLevel(cumulative);
        
        protected int GetPreviousLevel(int cumulative)
        {
            int currentLevel = LevelFromCumulative(cumulative);
            return Math.Max(MinLevel, currentLevel - 1);
        }

        protected int GetNextLevel(int cumulative)
        {
            int currentLevel = LevelFromCumulative(cumulative);
            return Math.Min(MaxLevel, currentLevel + 1);
        }
        
        protected abstract int LevelFromCumulative(int cumulative);
        protected abstract int CumulativeFromLevel(int level);
    }
}