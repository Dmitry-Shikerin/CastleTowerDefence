using System;
using Sources.BoundedContexts.ExplosionBodies.Presentation.Implementation;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Managers;
using UnityEngine;

namespace Sources.BoundedContexts.ExplosionBodies.Infrastructure.Factories.Views.Implementation
{
    public class ExplosionBodyViewFactory
    {
        private readonly IPoolManager _poolManager;

        public ExplosionBodyViewFactory(IPoolManager poolManager)
        {
            _poolManager = poolManager ?? throw new ArgumentNullException(nameof(poolManager));
        }
        
        public ExplosionBodyView Create(Vector3 position)
        {
            ExplosionBodyView view = _poolManager.Get<ExplosionBodyView>();
            view.SetPosition(position);
            
            return view;
        }
    }
}