using Sources.BoundedContexts.ExplosionBodies.Presentation.Interfaces;
using UnityEngine;

namespace Sources.BoundedContexts.ExplosionBodies.Infrastructure.Services.Spawners.Interfaces
{
    public interface IExplosionBodyBloodySpawnService
    {
        public IExplosionBodyBloodyView Spawn(Vector3 position);
    }
}