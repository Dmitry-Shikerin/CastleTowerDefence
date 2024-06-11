using Sources.BoundedContexts.EnemyKamikazes.Presentations.Interfaces;
using Sources.BoundedContexts.EnemySpawners.Domain.Models;
using Sources.BoundedContexts.KillEnemyCounters.Domain;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using UnityEngine;

namespace Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Services.Spawners.Interfaces
{
    public interface IEnemyKamikazeSpawnService
    {
        public IEnemyKamikazeView Spawn(
            KillEnemyCounter killEnemyCounter, PlayerWallet playerWallet, EnemySpawner enemySpawner, Vector3 position);
    }
}