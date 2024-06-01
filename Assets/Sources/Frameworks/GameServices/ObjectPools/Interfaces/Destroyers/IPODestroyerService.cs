using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;

namespace Sources.Frameworks.GameServices.ObjectPools.Interfaces.Destroyers
{
    public interface IPODestroyerService
    {
        public void Destroy<T>(T view)
            where T : View;
    }
}