using Sources.BoundedContexts.ExplosionBodies.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.ExplosionBodies.Presentation.Implementation;
using Sources.BoundedContexts.ExplosionBodies.Presentation.Interfaces;
using Sources.BoundedContexts.ObjectPools.Infrastructure.Factories;
using Sources.Domain.Models.Constants;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Generic;
using UnityEngine;

namespace Sources.BoundedContexts.ExplosionBodies.Infrastructure.Factories.Views.Implementation
{
    public class ExplosionBodyBloodyViewFactory : PoolableObjectFactory<ExplosionBodyBloodyView>, IExplosionBodyBloodyViewFactory
    {
        public ExplosionBodyBloodyViewFactory(IObjectPool<ExplosionBodyBloodyView> pool) 
            : base(pool)
        {
        }
        
        public IExplosionBodyBloodyView Create(
            ExplosionBodyBloodyView explosionBodyBloodyView, 
            Vector3 position)
        {
            return explosionBodyBloodyView;
        }
        
        public IExplosionBodyBloodyView Create(Vector3 position)
        {
            ExplosionBodyBloodyView explosionBodyBloodyView = CreateView(PrefabPath.ExplosionBodyBloody);
            
            return explosionBodyBloodyView;
        }
    }
}