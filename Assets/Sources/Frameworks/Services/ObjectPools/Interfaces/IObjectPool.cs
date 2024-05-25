using System;
using Sources.Presentations.Views;

namespace Sources.Frameworks.Services.ObjectPools
{
    public interface IObjectPool
    {
        event Action<int> ObjectCountChanged;
        
        T Get<T>()
            where T : View;
        void Return(PoolableObject poolableObject);
    }
}