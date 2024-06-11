using Sources.BoundedContexts.Enemies.Presentation;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.EnemySpawners.Domain.Models;
using Sources.BoundedContexts.KillEnemyCounters.Domain;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using UnityEngine;

namespace Sources.BoundedContexts.Enemies.Infrastructure.Services.Spawners.Interfaces
{
    public interface IEnemySpawnService
    {
        IEnemyView Spawn(KillEnemyCounter killEnemyCounter, EnemySpawner enemySpawner, PlayerWallet playerWallet, Vector3 position);
    }
}