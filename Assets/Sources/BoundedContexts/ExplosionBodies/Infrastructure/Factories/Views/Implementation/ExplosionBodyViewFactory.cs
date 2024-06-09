using Sources.BoundedContexts.ExplosionBodies.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.ExplosionBodies.Presentation.Implementation;
using Sources.BoundedContexts.ExplosionBodies.Presentation.Interfaces;
using Sources.BoundedContexts.ObjectPools.Infrastructure.Factories;
using Sources.BoundedContexts.Prefabs;
using Sources.Domain.Models.Constants;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Generic;
using UnityEngine;

namespace Sources.BoundedContexts.ExplosionBodies.Infrastructure.Factories.Views.Implementation
{
    public class ExplosionBodyViewFactory : PoolableObjectFactory<ExplosionBodyView>, IExplosionBodyViewFactory
    {
        public ExplosionBodyViewFactory(IObjectPool<ExplosionBodyView> pool) 
            : base(pool)
        {
        }
        
        public IExplosionBodyView Create(
            ExplosionBodyView view, 
            Vector3 position)
        {
            return view;
        }
        
        public IExplosionBodyView Create(Vector3 position)
        {
            ExplosionBodyView view = CreateView(PrefabPath.ExplosionBody);
            
            return view;
        }
    }
}