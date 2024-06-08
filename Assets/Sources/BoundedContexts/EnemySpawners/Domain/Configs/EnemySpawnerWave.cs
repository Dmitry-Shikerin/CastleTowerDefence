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
        [SerializeField] private EnemySpawnerConfigContainer _parent;
        [EnableIf("_enable", Enable.Enable)]
        [SerializeField] private int _waveId;
        [Space(10)] 
        [Header("Enemy")]
        [SerializeField] private int _spawnDelay;
        [SerializeField] private int _enemyCount;       
        [SerializeField] private int _moneyPerResilenceCharacters;
        [SerializeField] private int _enemyAttackPower;
        [SerializeField] private int _enemyHealth;
        [SerializeField] private int _enemyAttackSpeed;
        [Header("Boss")]
        [SerializeField] private int _bossesCount;
        [SerializeField] private int _bossAttackPower;
        [SerializeField] private int _bossHealth;
        [SerializeField] private int _bossAttackSpeed;
        [Header("Runners")]
        [SerializeField] private int _rannerEnemyCount;

        
        public int WaveId => _waveId;
        public int SpawnDelay => _spawnDelay;
        public int EnemyCount => _enemyCount;

        public EnemySpawnerConfigContainer Parent
        {
            get => _parent; 
            set => _parent = value;
        }
        
        public void SetWaveId(int id) =>
            _waveId = id;
        
        [Button(ButtonSizes.Medium)] [PropertySpace(20)]
        private void Remove() =>
            Parent.RemoveWave(this);
    }
}