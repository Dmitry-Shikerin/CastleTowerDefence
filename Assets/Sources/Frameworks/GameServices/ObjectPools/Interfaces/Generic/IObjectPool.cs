using System.Collections.Generic;
using Sources.Frameworks.GameServices.ObjectPools.Implementation.Bakers;
using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.Views;
using Sources.PresentationsInterfaces.Views;

namespace Sources.Frameworks.GameServices.ObjectPools.Interfaces.Generic
{
    public interface IObjectPool<T> : IObjectPool
        where T : IView
    {
        IReadOnlyList<T> Collection { get; }
        IPoolBaker<T> PoolBaker { get; }

        void AddToCollection(T @object);
        bool Contains(T @object);
    }
}