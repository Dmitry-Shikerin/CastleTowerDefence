using System;
using Sources.BoundedContexts.ExplosionBodies.Presentation.Implementation;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Managers;
using UnityEngine;

namespace Sources.BoundedContexts.ExplosionBodies.Infrastructure.Factories.Views.Implementation
{
    public class ExplosionBodyBloodyViewFactory
    {
        private readonly IPoolManager _poolManager;

        public ExplosionBodyBloodyViewFactory(IPoolManager poolManager)
        {
            _poolManager = poolManager ?? throw new ArgumentNullException(nameof(poolManager));
        }
        
        public ExplosionBodyBloodyView Create(Vector3 position)
        {
            ExplosionBodyBloodyView view = _poolManager
                .Get<ExplosionBodyBloodyView>();
            
            view.SetPosition(position);
            view.Show();
            
            view.SetPosition(position);
            view.Show();
            
            return view;
        }
    }
}