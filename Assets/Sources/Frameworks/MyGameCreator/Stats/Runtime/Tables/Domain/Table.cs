using Sources.Frameworks.MyGameCreator.Core.Runtime.Common;
using Sources.Frameworks.MyGameCreator.Stats.Tables.Domain.Types;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sources.Frameworks.MyGameCreator.Stats.Tables.Domain
{
    [Icon("Assets/Sources/Frameworks/MyGameCreator/Stats/Editor/Gizmos/GizmoTable.png")]
    [CreateAssetMenu(fileName = "Table", menuName = "Configs/Stats/Tables", order = 51)]
    public class Table : ScriptableObject
    {
        //   +-----------------+
        //   |   TABLE CHART   |
        //   +-----------------+
        // 
        //   +--------------+--+
        //   |              |  |
        //   |           +--+  |
        //   |           |  |  | <- Each bar represents the Level Experience to go from that Level
        //   |        +--+  |  |    to the next one.
        //   |        |  |  |  |
        //   |     +--+  |  |  |    Level Experience: Amount of experience needed to go from a
        //   |     |  |  |  |  |    to the next one.
        //   |  +--+  |  |  |  |    
        //   |  |  |  |  |  |  |    Cumulative Experience: The sum of Level Experience values
        //   |--+  |  |  |  |  |    from all previous levels.
        //   |  |  |  |  |  |  |
        //   +--+--+--+--+--+--+
        //   |1 |2 |3 |4 |5 |6 | <- Each number represents a Level
        
        [SerializeReference] private TTable _table = new TableLinearProgression();


        /// <summary>
        /// Returns the level from the cumulative experience value provided.
        /// </summary>
        /// <param name="experience"></param>
        /// <returns></returns>
        public int CurrentLevel(int experience) =>
            this._table?.GetLevelForCumulativeExperience(experience) ?? 0;

        /// <summary>
        /// Returns the step-experience value between the the current and the next level.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public int ExperienceForLevel(int level)
        {
            return this._table?.GetLevelExperienceForLevel(level) ?? 0;
        }

        /// <summary>
        /// Returns the cumulative experience for the specified level.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public int CumulativeExperienceForLevel(int level)
        {
            return this._table?.GetCumulativeExperienceForLevel(level) ?? 0;
        }

        /// <summary>
        /// Returns the amount of experience accumulated at the current level from the cumulative
        /// experience value provided.
        /// </summary>
        /// <param name="cumulative"></param>
        /// <returns></returns>
        public int ExperienceForCurrentLevel(int cumulative)
        {
            return this._table?.GetLevelExperienceAtCurrentLevel(cumulative) ?? 0;
        }
        
        /// <summary>
        /// Returns the amount of experience left from the cumulative experience value to reach
        /// the next level.
        /// </summary>
        /// <param name="cumulative"></param>
        /// <returns></returns>
        public int ExperienceToNextLevel(int cumulative)
        {
            return this._table?.GetLevelExperienceToNextLevel(cumulative) ?? 0;
        }
        
        /// <summary>
        /// Returns a value between 0 and 1 indicating the percentage of experience accumulated at
        /// the current level.
        /// </summary>
        /// <param name="experience"></param>
        /// <returns></returns>
        public float RatioFromCurrentLevel(int experience)
        {
            return this._table?.GetRatioAtCurrentLevel(experience) ?? 0f;
        }

        /// <summary>
        /// Returns a value between 0 and 1 indicating the percentage of experience left to reach
        /// the next level.
        /// </summary>
        /// <param name="experience"></param>
        /// <returns></returns>
        public float RatioForNextLevel(int experience)
        {
            return this._table?.GetRatioForNextLevel(experience) ?? 0f;
        }
    }
}