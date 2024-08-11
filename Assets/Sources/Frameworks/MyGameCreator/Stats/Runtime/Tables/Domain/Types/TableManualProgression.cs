using System;
using Sources.Frameworks.MyGameCreator.Core.Runtime.Common;
using Sources.Frameworks.MyGameCreator.Stats.Runtime.Tables;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sources.Frameworks.MyGameCreator.Stats.Tables.Domain.Types
{
    [Image(typeof(IconTable), ColorTheme.Type.Yellow)]
    [Title("Manual Progression")]
    [Category("Manual Progression")]
    [Description("Manually defines the amount of experience needed to reach the next level")]
    [Serializable]
    public class TableManualProgression : TTable
    {
        // +--------------------------------------------------------------------------------------+
        // | EXP_Level(n) = X(n)                                                                  |
        // |                                                                                      |
        // | n: is the current level.                                                             |
        // | X(n): is the n-th variable from the experience table                                 |
        // +--------------------------------------------------------------------------------------+
        
        [SerializeField] private int[] _increments = new int[99];
        
        public override int MinLevel => 1;
        public override int MaxLevel => _increments.Length - 1;

        public TableManualProgression() : base()
        {
            for (int i = 0; i < _increments.Length; ++i)
            {
                _increments[i] = 10 + 5 * i;
            }
        }

        public TableManualProgression(int[] increments) : this()
        {
            _increments = increments;
        }
        
        // IMPLEMENT METHODS: ---------------------------------------------------------------------

        protected override int LevelFromCumulative(int cumulative)
        {
            int sum = 0;
            
            for (int i = 0; i < _increments.Length; ++i)
            {
                if (cumulative < sum) return i;
                sum += _increments[i];
            }

            return MaxLevel;
        }

        protected override int CumulativeFromLevel(int level)
        {
            int sum = 0;
            for (int i = 0; i < level - 1; ++i)
            {
                sum += _increments[i];
            }

            return sum;
        }
    }
}