using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;

namespace Sources.Frameworks.GameServices.ObjectPools.Implementation.Managers
{
    public interface IPoolManager
    {
        T Get<T>(string resourcesPath) 
            where T : View;
    }
}