using Sources.BoundedContexts.CharacterHealth.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.CharacterHealth.Infrastructure.Factories.Views;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Factories.Providers;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Factories.Views.Implementation;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Spawners.Implementation;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.CharacterMelees.Presentation.Implementation;
using Sources.BoundedContexts.CharacterRanges.Infrastructure.Factories.Services;
using Sources.BoundedContexts.CharacterRanges.Infrastructure.Factories.Views.Implementation;
using Sources.BoundedContexts.CharacterRanges.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.CharacterRanges.Infrastructure.Services.Spawners.Implementation;
using Sources.BoundedContexts.CharacterRanges.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.CharacterRanges.Presentation.Implementation;
using Sources.BoundedContexts.CharacterRotations.Services.Implementation;
using Sources.BoundedContexts.CharacterRotations.Services.Interfaces;
using Sources.BoundedContexts.CharacterSpawners.Ifrastructure.Factories.Controllers;
using Sources.BoundedContexts.CharacterSpawners.Ifrastructure.Factories.Views;
using Sources.Frameworks.GameServices.ObjectPools.Implementation;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Generic;
using Zenject;

namespace Sources.App.DIContainers.Gameplay
{
    public class CharacterInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<CharacterSpawnerPresenterFactory>().AsSingle();
            Container.Bind<CharacterSpawnerViewFactory>().AsSingle();

            Container.Bind<ICharacterRotationService>().To<CharacterRotationService>().AsSingle();
            
            Container.Bind<IObjectPool<CharacterMeleeView>>().To<ObjectPool<CharacterMeleeView>>().AsSingle();
            Container.Bind<ICharacterMeleeSpawnService>().To<CharacterMeleeSpawnService>().AsSingle();
            Container.Bind<CharacterMeleeDependencyProviderFactory>().AsSingle();
            Container.Bind<ICharacterMeleeViewFactory>().To<CharacterMeleeViewFactory>().AsSingle();

            Container.Bind<IObjectPool<CharacterRangeView>>().To<ObjectPool<CharacterRangeView>>().AsSingle();
            Container.Bind<ICharacterRangeSpawnService>().To<CharacterRangeSpawnService>().AsSingle();
            Container.Bind<CharacterRangeDependencyProviderFactory>().AsSingle();
            Container.Bind<ICharacterRangeViewFactory>().To<CharacterRangeViewFactory>().AsSingle();

            Container.Bind<CharacterHealthPresenterFactory>().AsSingle();
            Container.Bind<CharacterHealthViewFactory>().AsSingle();
        }
    }
}