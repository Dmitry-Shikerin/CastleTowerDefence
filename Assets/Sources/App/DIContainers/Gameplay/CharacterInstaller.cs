using Sources.BoundedContexts.CharacterHealth.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.CharacterHealth.Infrastructure.Factories.Views;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Factories.Views.Implementation;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Spawners.Implementation;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.Characters.Infrastructure.Factories.Providers;
using Sources.BoundedContexts.CharacterSpawners.Ifrastructure.Factories.Views;
using Zenject;

namespace Sources.App.DIContainers.Gameplay
{
    public class CharacterInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<CharacterSpawnerViewFactory>().AsSingle();

            Container.Bind<ICharacterMeleeSpawnService>().To<CharacterMeleeSpawnService>().AsSingle();
            Container.Bind<CharacterMeleeDependencyProviderFactory>().AsSingle();
            Container.Bind<CharacterMeleeViewFactory>().AsSingle();

            Container.Bind<CharacterHealthPresenterFactory>().AsSingle();
            Container.Bind<CharacterHealthViewFactory>().AsSingle();
        }
    }
}