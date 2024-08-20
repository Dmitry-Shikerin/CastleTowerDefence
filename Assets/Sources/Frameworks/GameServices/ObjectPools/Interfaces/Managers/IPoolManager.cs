using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Generic;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.Views;

namespace Sources.Frameworks.GameServices.ObjectPools.Interfaces.Managers
{
    public interface IPoolManager
    {
        T Get<T>() 
            where T : View;
        IObjectPool<T> GetPool<T>() 
            where T : IView;
        public bool Contains<T>(T @object) 
            where T : View;
    }
}