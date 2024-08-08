using System;
using Sources.BoundedContexts.ExplosionBodies.Presentation.Implementation;
using Sources.BoundedContexts.ExplosionBodies.Presentation.Interfaces;
using Sources.BoundedContexts.Prefabs;
using Sources.Frameworks.GameServices.ObjectPools.Implementation.Managers;
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
        
        public IExplosionBodyView Create(
            ExplosionBodyView view, 
            Vector3 position)
        {
            return view;
        }
        
        public IExplosionBodyView Create(Vector3 position)
        {
            ExplosionBodyView view = _poolManager.Get<ExplosionBodyView>(PrefabPath.ExplosionBody);
            
            return view;
        }
    }
}