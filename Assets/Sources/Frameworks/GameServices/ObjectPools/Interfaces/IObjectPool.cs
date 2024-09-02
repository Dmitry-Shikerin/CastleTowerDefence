using System;
using Sources.Frameworks.GameServices.ObjectPools.Implementation.Objects;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;

namespace Sources.Frameworks.GameServices.ObjectPools.Interfaces
{
    public interface IObjectPool
    {
        event Action<int> ObjectCountChanged;
        
        float DeleteAfterTime { get; }

        void RemoveFromPool(PoolableObject poolableObject);
        T Get<T>()
            where T : View;
        void Return(PoolableObject poolableObject);
        void PoolableObjectDestroyed(PoolableObject poolableObject);
    }
}