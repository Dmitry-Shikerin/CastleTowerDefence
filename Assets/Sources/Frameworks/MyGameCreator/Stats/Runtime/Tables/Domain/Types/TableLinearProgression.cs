using System;
using Sources.Frameworks.MyGameCreator.Core.Runtime.Common;
using Sources.Frameworks.MyGameCreator.Stats.Runtime.Tables;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sources.Frameworks.MyGameCreator.Stats.Tables.Domain.Types
{
    [Image(typeof(IconTable), ColorTheme.Type.Green)]
    [Title("Linear Progression")]
    [Category("Linear Progression")]
    [Description(
        "The experience needed to reach the next level is equal to the previous level " +
        "multiplied by a constant increment")]
    [Serializable]
    public class TableLinearProgression : TTable
    {
        // +--------------------------------------------------------------------------------------+
        // | EXP_Level(n + 1) = EXP_Level(n) + (n * increment)                                    |
        // |                                                                                      |
        // | n: is the current level.                                                             |
        // | increment: is the amount of experience added per level.                              |
        // +--------------------------------------------------------------------------------------+
        
        [SerializeField] private int _maxLevel = 99;
        [SerializeField] private int _incrementPerLevel = 50;
        
        public override int MinLevel => 1;
        public override int MaxLevel => _maxLevel;
        
        public TableLinearProgression() : base()
        { }

        public TableLinearProgression(int maxLevel, int incrementPerLevel) : this()
        {
            _maxLevel = maxLevel;
            _incrementPerLevel = incrementPerLevel;
        }
        
        protected override int LevelFromCumulative(int cumulative)
        {
            float squareRoot = Mathf.Sqrt(1f + 8f * cumulative / _incrementPerLevel);
            float level = (1 + squareRoot) / 2.0f;
            
            return Mathf.Clamp(Mathf.FloorToInt(level), MinLevel, MaxLevel + 1);
        }

        protected override int CumulativeFromLevel(int level)
        {
            float power = Mathf.Pow(level, 2.0f);
            float result = (power - level) * _incrementPerLevel / 2.0f;
            
            return Mathf.FloorToInt(result);
        }
    }
}