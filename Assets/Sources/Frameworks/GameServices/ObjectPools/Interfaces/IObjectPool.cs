using System;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
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