using Sources.PresentationsInterfaces.Views;

namespace Sources.Frameworks.Services.ObjectPools.Generic
{
    public interface IObjectPool<T> : IObjectPool
        where T : IView
    {
    }
}