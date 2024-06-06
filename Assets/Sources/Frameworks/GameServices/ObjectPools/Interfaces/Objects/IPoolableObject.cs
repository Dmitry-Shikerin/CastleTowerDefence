namespace Sources.Frameworks.GameServices.ObjectPools.Interfaces.Objects
{
    public interface IPoolableObject
    {
        void SetPool(IObjectPool pool);
        void ReturnToPool();
    }
}