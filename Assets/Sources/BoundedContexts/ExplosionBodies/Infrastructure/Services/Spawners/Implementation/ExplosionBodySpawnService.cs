using System;
using Sources.BoundedContexts.ExplosionBodies.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.ExplosionBodies.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.ExplosionBodies.Presentation.Implementation;
using Sources.BoundedContexts.ExplosionBodies.Presentation.Interfaces;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Generic;
using UnityEngine;

namespace Sources.BoundedContexts.ExplosionBodies.Infrastructure.Services.Spawners.Implementation
{
    public class ExplosionBodySpawnService : IExplosionBodySpawnService
    {
        private readonly IObjectPool<ExplosionBodyView> _objectPool;
        private readonly IExplosionBodyViewFactory _viewFactory;

        public ExplosionBodySpawnService(
            IObjectPool<ExplosionBodyView> objectPool,
            IExplosionBodyViewFactory viewFactory)
        {
            _objectPool = objectPool ?? throw new ArgumentNullException(nameof(objectPool));
            _viewFactory = viewFactory ?? throw new ArgumentNullException(nameof(viewFactory));
        }

        public IExplosionBodyView Spawn(Vector3 position)
        {
            IExplosionBodyView view = SpawnFromPool(position) ?? _viewFactory.Create(position);

            view.SetPosition(position);
            view.Show();

            return view;
        }

        private IExplosionBodyView SpawnFromPool(Vector3 position)
        {
            ExplosionBodyView explosionBodyBloodyView = _objectPool.Get<ExplosionBodyView>();

            if (explosionBodyBloodyView == null)
                return null;

            return _viewFactory.Create(explosionBodyBloodyView, position);
        }
    }
}