using Sources.Frameworks.GameServices.ObjectPools.Interfaces;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Objects;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;

namespace Sources.Frameworks.GameServices.ObjectPools.Implementation.Objects
{
    public class PoolableObject : View, IPoolableObject
    {
        private IObjectPool _pool;

        private void OnDestroy() =>
            _pool.PoolableObjectDestroyed(this);

        public void SetPool(IObjectPool pool) =>
            _pool = pool;

        public void ReturnToPool() =>
            _pool.Return(this);
    }
}