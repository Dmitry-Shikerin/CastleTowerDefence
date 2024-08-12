using System;
using Sources.Frameworks.MyGameCreator.Core.Runtime.Common;
using Sources.Frameworks.MyGameCreator.Stats.Runtime.Tables;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sources.Frameworks.MyGameCreator.Stats.Tables.Domain.Types
{
    [Image(typeof(IconTable), ColorTheme.Type.Blue)]
    [Title("Constant Progression")]
    [Category("Constant Progression")]
    [Description("Each level requires the same amount of experience to reach the next one")]
    [Serializable] 
    public class TableConstantProgression : TTable
    {
        // +--------------------------------------------------------------------------------------+
        // | EXP_Level(n + 1) = experience                                                        |
        // |                                                                                      |
        // | n: is the current level.                                                             |
        // | experience: is the amount of experience required per level.                          |
        // +--------------------------------------------------------------------------------------+
        
        [SerializeField] private int _maxLevel = 99;
        [SerializeField] private int _increment = 50;
        
        public override int MinLevel => 1;
        public override int MaxLevel => _maxLevel;

        public TableConstantProgression() : base()
        { }

        public TableConstantProgression(int maxLevel, int increment) : this()
        {
            _maxLevel = maxLevel;
            _increment = increment;
        }
        
        protected override int LevelFromCumulative(int cumulative)
        {
            float level = (float) (cumulative + _increment) / _increment;
            return Mathf.Clamp(Mathf.FloorToInt(level), MinLevel, MaxLevel + 1);
        }

        protected override int CumulativeFromLevel(int level)
        {
            return level * _increment - _increment;
        }
    }
}