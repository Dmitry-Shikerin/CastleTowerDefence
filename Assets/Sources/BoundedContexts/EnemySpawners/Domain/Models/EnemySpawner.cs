using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sources.BoundedContexts.KillEnemyCounters.Domain;

namespace Sources.BoundedContexts.EnemySpawners.Domain.Models
{
    public class EnemySpawner
    {
        public bool IsSpawnEnemy { get; set; } = true;
        public int SpawnedEnemies { get; set; }
        public bool IsSpawnBoss { get; set; }
        public int SpawnedBosses { get; set; }
        public List<int> SpawnDelays { get; set; }
        public int CurrentWave { get; set; }

        public UniTask WaitWave(KillEnemyCounter killEnemyCounter, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public void SetCurrentWave(int killZombies)
        {
            throw new System.NotImplementedException();
        }
    }
}