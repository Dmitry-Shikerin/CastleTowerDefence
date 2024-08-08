using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.Views;

namespace Sources.Frameworks.GameServices.ObjectPools.Interfaces.Bakers
{
    public interface IPoolBaker
    {
        void Bake();
        void Add<T>(T gameObject) 
            where T : IView;
    }
}