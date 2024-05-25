namespace Sources.Frameworks.Services.ObjectPools
{
    public interface IPoolableObject
    {
        void SetPool(IObjectPool pool);
        void ReturnToPool();
    }
}