using System.Collections.Generic;
using UnityEngine;

namespace Sources.Frameworks.MyGameCreator.Achivements.Domain.Configs
{
    [CreateAssetMenu(
        fileName = "AchievementConfigCollector", 
        menuName = "Configs/Achievements/AchievementConfigCollector", 
        order = 51)]
    public class AchievementConfigCollector : ScriptableObject
    {
        [field: SerializeField] public List<AchievementConfig> Configs { get; private set; }
    }
}