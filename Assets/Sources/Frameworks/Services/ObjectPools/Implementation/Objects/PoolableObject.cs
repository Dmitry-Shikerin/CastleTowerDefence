using Sources.Presentations.Views;

namespace Sources.Frameworks.Services.ObjectPools
{
    public class PoolableObject : View, IPoolableObject
    {
        private IObjectPool _pool;

        public void SetPool(IObjectPool pool) =>
            _pool = pool;

        public void ReturnToPool() =>
            _pool.Return(this);
    }
}