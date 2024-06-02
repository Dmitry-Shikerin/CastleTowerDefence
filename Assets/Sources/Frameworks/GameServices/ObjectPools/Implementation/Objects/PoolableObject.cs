using Sources.Frameworks.GameServices.ObjectPools.Interfaces;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using Sources.Frameworks.Services.ObjectPools;

namespace Sources.Frameworks.GameServices.ObjectPools.Implementation.Objects
{
    public class PoolableObject : View, IPoolableObject
    {
        private IObjectPool _pool;

        private void OnDestroy() =>
            _pool.PoolableObjectDestroyed();

        public void SetPool(IObjectPool pool) =>
            _pool = pool;

        public void ReturnToPool() =>
            _pool.Return(this);
    }
}