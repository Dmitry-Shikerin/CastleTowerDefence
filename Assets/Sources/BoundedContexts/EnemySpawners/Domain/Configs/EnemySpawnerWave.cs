using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sources.Frameworks.UiFramework.Core.Presentation.CommonTypes;
using UnityEngine;

namespace Sources.BoundedContexts.EnemySpawners.Domain.Configs
{
    [CreateAssetMenu(fileName = "EnemySpawnerWave", menuName = "Configs/EnemySpawnerWave", order = 51)]
    public class EnemySpawnerWave : ScriptableObject
    {
        [EnumToggleButtons] [HideLabel] [UsedImplicitly]
        [SerializeField] private Enable _enable = Enable.Disable;
        [EnableIf("_enable", Enable.Enable)]
        [SerializeField] private int _waveId;
        [SerializeField] private int _spawnDelay;
        [SerializeField] private int _enemyCount;
        
        public int WaveId => _waveId;
        public int SpawnDelay => _spawnDelay;
        public int EnemyCount => _enemyCount;
        
        public void SetWaveId(int id) =>
            _waveId = id;
    }
}