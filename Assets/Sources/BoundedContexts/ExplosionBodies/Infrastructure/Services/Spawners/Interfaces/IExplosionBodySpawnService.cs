using Sources.BoundedContexts.ExplosionBodies.Presentation.Interfaces;
using UnityEngine;

namespace Sources.BoundedContexts.ExplosionBodies.Infrastructure.Services.Spawners.Interfaces
{
    public interface IExplosionBodySpawnService
    {
        IExplosionBodyView Spawn(Vector3 position);
    }
}