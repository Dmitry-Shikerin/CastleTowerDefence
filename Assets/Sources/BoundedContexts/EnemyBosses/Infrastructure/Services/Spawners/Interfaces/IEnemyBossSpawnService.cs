using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;
using Sources.BoundedContexts.EnemySpawners.Domain.Models;
using Sources.BoundedContexts.KillEnemyCounters.Domain;
using UnityEngine;

namespace Sources.BoundedContexts.EnemyBosses.Infrastructure.Services.Spawners.Interfaces
{
    public interface IEnemyBossSpawnService
    {
        IEnemyBossView Spawn(KillEnemyCounter killEnemyCounter, EnemySpawner enemySpawner, Vector3 position);
    }
}