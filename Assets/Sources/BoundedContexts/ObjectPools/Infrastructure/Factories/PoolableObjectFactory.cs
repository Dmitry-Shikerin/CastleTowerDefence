using System;
using Sources.Frameworks.GameServices.ObjectPools.Implementation.Objects;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Generic;
using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.Views;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sources.BoundedContexts.ObjectPools.Infrastructure.Factories
{
    public abstract class PoolableObjectFactory<T>
        where T : MonoBehaviour, IView
    {
        private readonly IObjectPool<T> _pool;

        protected PoolableObjectFactory(IObjectPool<T> pool)
        {
            _pool = pool ?? throw new ArgumentNullException(nameof(pool));
        }

        protected T CreateView(string prefabPath)
        {
            T enemyView = Object.Instantiate(
                Resources.Load<T>(prefabPath));

            enemyView
                .AddComponent<PoolableObject>()
                .SetPool(_pool);

            _pool.AddToCollection(enemyView);

            return enemyView;
        }
    }
}