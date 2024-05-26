using Sources.BoundedContexts.CharacterHealth.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.CharacterHealth.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Characters.Infrastructure.Factories.Providers;
using Sources.BoundedContexts.Characters.Infrastructure.Factories.Views;
using Zenject;

namespace Sources.App.DIContainers.Gameplay
{
    public class CharacterInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<CharacterDependencyProviderFactory>().AsSingle();
            Container.Bind<CharacterViewFactory>().AsSingle();

            Container.Bind<CharacterHealthPresenterFactory>().AsSingle();
            Container.Bind<CharacterHealthViewFactory>().AsSingle();
        }
    }
}