using Sources.BoundedContexts.Enemies.Presentation;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.KillEnemyCounters.Domain;
using UnityEngine;

namespace Sources.BoundedContexts.Enemies.Infrastructure.Services.Spawners.Interfaces
{
    public interface IEnemySpawnService
    {
        IEnemyView Spawn(KillEnemyCounter killEnemyCounter, Vector3 position);
    }
}