using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Bakers;
using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.Views;

namespace Sources.Frameworks.GameServices.ObjectPools.Implementation.Bakers
{
    public interface IPoolBaker<T> : IPoolBaker
        where T : IView
    {
    }
}