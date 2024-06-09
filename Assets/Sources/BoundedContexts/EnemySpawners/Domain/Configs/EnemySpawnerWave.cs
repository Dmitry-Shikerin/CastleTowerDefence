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
        [SerializeField] private int _moneyPerResilenceCharacters;
        [Space(10)] 
        [Header("Wave")]
        [SerializeField] private int _spawnDelay;
        [Header("Enemy")]
        [SerializeField] private int _enemyCount;       
        [SerializeField] private int _enemyAttackPower;
        [SerializeField] private int _enemyHealth;
        [SerializeField] private int _enemyAttackSpeed;
        [Header("Boss")]
        [SerializeField] private int _bossesCount;
        [SerializeField] private int _bossAttackPower;
        [SerializeField] private int _bossMassAttackPower;
        [SerializeField] private int _bossHealth;
        [SerializeField] private int _bossAttackSpeed;
        [Header("Kamikaze")]
        [SerializeField] private int _kamikazeEnemyCount;
        [SerializeField] private int _kamikazeMassAttackPower;
        
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