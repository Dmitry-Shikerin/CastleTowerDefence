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
        [SerializeField] private EnemySpawnerConfig _parent;
        [EnableIf("_enable", Enable.Enable)]
        [SerializeField] private int _waveId;
        [SerializeField] private int _moneyPerResilenceCharacters;
        [Space(10)] 
        [Header("Wave")]
        [SerializeField] private int _spawnDelay;
        [Header("Enemy")]
        [SerializeField] private int _enemyCount;       
        [Header("Boss")]
        [SerializeField] private int _bossesCount;
        [Header("Kamikaze")]
        [SerializeField] private int _kamikazeEnemyCount;

        
        //Enemys
        public int WaveId => _waveId;
        public int SpawnDelay => _spawnDelay;
        public int EnemyCount => _enemyCount;
        
        //Bosses
        public int BossesCount => _bossesCount;
        
        //Kamikaze
        public int KamikazeEnemyCount => _kamikazeEnemyCount;
        
        //Money
        public int MoneyPerResilenceCharacters => _moneyPerResilenceCharacters;
        
        //Common
        public int SumEnemies => _enemyCount + _bossesCount + _kamikazeEnemyCount;

        public EnemySpawnerConfig Parent
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