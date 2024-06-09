using Sources.BoundedContexts.EnemyKamikazes.Presentations.Interfaces;
using Sources.BoundedContexts.KillEnemyCounters.Domain;
using UnityEngine;

namespace Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Services.Spawners.Interfaces
{
    public interface IEnemyKamikazeSpawnService
    {
        public IEnemyKamikazeView Spawn(KillEnemyCounter killEnemyCounter, Vector3 position);
    }
}