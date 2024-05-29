using Sources.BoundedContexts.Healths.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.Healths.Infrastructure.Factories.Views;
using Zenject;

namespace Sources.App.DIContainers.Gameplay
{
    public class CommonGameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<HealthBarPresenterFactory>().AsSingle();
            Container.Bind<HealthBarViewFactory>().AsSingle();
        }
    }
}