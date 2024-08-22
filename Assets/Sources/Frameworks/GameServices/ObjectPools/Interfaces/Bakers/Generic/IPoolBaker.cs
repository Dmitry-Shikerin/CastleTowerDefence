using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.Views;

namespace Sources.Frameworks.GameServices.ObjectPools.Interfaces.Bakers.Generic
{
    public interface IPoolBaker<T> : IPoolBaker
        where T : IView
    {
    }
}