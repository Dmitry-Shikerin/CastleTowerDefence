using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.EnemySpawners.Domain.Models;
using UnityEngine;

namespace Sources.BoundedContexts.Enemies.Infrastructure.Services.Spawners.Interfaces
{
    public interface IEnemySpawnService
    {
        IEnemyView Spawn(EnemySpawner enemySpawner, Vector3 position);
    }
}