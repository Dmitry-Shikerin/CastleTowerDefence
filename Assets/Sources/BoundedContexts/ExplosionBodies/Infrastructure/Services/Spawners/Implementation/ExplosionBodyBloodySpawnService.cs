using System;
using Sources.BoundedContexts.ExplosionBodies.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.ExplosionBodies.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.ExplosionBodies.Presentation.Implementation;
using Sources.BoundedContexts.ExplosionBodies.Presentation.Interfaces;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Generic;
using UnityEngine;

namespace Sources.BoundedContexts.ExplosionBodies.Infrastructure.Services.Spawners.Implementation
{
    public class ExplosionBodyBloodySpawnService : IExplosionBodyBloodySpawnService
    {
        private readonly IObjectPool<ExplosionBodyBloodyView> _objectPool;
        private readonly IExplosionBodyBloodyViewFactory _viewFactory;

        public ExplosionBodyBloodySpawnService(
            IObjectPool<ExplosionBodyBloodyView> objectPool,
            IExplosionBodyBloodyViewFactory viewFactory)
        {
            _objectPool = objectPool ?? throw new ArgumentNullException(nameof(objectPool));
            _viewFactory = viewFactory ?? throw new ArgumentNullException(nameof(viewFactory));
        }

        public IExplosionBodyBloodyView Spawn(Vector3 position)
        {
            IExplosionBodyBloodyView view = SpawnFromPool(position) ?? _viewFactory.Create(position);

            view.SetPosition(position);
            view.Show();

            return view;
        }

        private IExplosionBodyBloodyView SpawnFromPool(Vector3 position)
        {
            ExplosionBodyBloodyView explosionBodyBloodyView = _objectPool.Get<ExplosionBodyBloodyView>();

            if (explosionBodyBloodyView == null)
                return null;

            return _viewFactory.Create(explosionBodyBloodyView, position);
        }
    }
}