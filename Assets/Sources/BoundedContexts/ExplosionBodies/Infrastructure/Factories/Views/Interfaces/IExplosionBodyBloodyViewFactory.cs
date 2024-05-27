using Sources.BoundedContexts.ExplosionBodies.Presentation.Implementation;
using Sources.BoundedContexts.ExplosionBodies.Presentation.Interfaces;
using UnityEngine;

namespace Sources.BoundedContexts.ExplosionBodies.Infrastructure.Factories.Views.Interfaces
{
    public interface IExplosionBodyBloodyViewFactory
    {
        IExplosionBodyBloodyView Create(ExplosionBodyBloodyView explosionBodyBloodyView, Vector3 position);
        IExplosionBodyBloodyView Create(Vector3 position);
    }
}