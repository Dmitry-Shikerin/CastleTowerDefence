using System;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Generic;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using Sources.Frameworks.Services.ObjectPools;

namespace Sources.Frameworks.GameServices.ObjectPools.Implementation.Objects
{
    public class PoolableObject : View, IPoolableObject
    {
        private IObjectPool _pool;

        public void SetPool(IObjectPool pool) =>
            _pool = pool;

        public void ReturnToPool() =>
            _pool.Return(this);

        public void RemoveFromObjectPool<T>(T view) 
            where T : View
        {
            if (gameObject.TryGetComponent(out T @object) == false)
                throw new NullReferenceException(nameof(T));
            
            (_pool as IObjectPool<T>)?.RemoveFromCollection(view);
        }
    }
}