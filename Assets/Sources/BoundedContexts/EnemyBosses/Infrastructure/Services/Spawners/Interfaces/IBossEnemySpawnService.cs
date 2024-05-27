using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;
using Sources.BoundedContexts.KillEnemyCounters.Domain;
using UnityEngine;

namespace Sources.BoundedContexts.EnemyBosses.Infrastructure.Services.Spawners.Interfaces
{
    public interface IBossEnemySpawnService
    {
        IBossEnemyView Spawn(KillEnemyCounter killEnemyCounter, Vector3 position);
    }
}