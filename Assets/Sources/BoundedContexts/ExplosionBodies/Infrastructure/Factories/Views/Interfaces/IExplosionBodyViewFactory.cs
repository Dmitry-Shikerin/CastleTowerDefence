using Sources.BoundedContexts.ExplosionBodies.Presentation.Implementation;
using Sources.BoundedContexts.ExplosionBodies.Presentation.Interfaces;
using UnityEngine;

namespace Sources.BoundedContexts.ExplosionBodies.Infrastructure.Factories.Views.Interfaces
{
    public interface IExplosionBodyViewFactory
    {
        IExplosionBodyView Create(ExplosionBodyView view, Vector3 position);
        IExplosionBodyView Create(Vector3 position);
    }
}