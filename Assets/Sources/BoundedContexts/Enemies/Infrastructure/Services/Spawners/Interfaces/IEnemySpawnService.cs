using Sources.BoundedContexts.Enemies.Presentation;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.EnemySpawners.Domain.Models;
using Sources.BoundedContexts.KillEnemyCounters.Domain;
using UnityEngine;

namespace Sources.BoundedContexts.Enemies.Infrastructure.Services.Spawners.Interfaces
{
    public interface IEnemySpawnService
    {
        IEnemyView Spawn(KillEnemyCounter killEnemyCounter, EnemySpawner enemySpawner, Vector3 position);
    }
}