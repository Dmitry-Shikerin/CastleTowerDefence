using Sirenix.OdinInspector;
using Sources.BoundedContexts.Huds.Presentations;
using Sources.BoundedContexts.RootGameObjects.Presentation;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Controllers.Implementation;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Controllers.Interfaces;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Domain.Implementation;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views.Implementation;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views.Interfaces;
using Sources.ECSBoundedContexts.StarUps;
using Sources.ECSBoundedContexts.StarUps.Implementation;
using Sources.ECSBoundedContexts.StarUps.Interfaces;
using UnityEngine;
using Zenject;

namespace Sources.App.DIContainers.Gameplay
{
    public class GameplayInstaller : MonoInstaller
    {
        [Required] [SerializeField] private RootGameObject _rootGameObject;
        [Required] [SerializeField] private GameplayHud _gameplayHud;
        
        public override void InstallBindings()
        {
            Container.Bind<RootGameObject>().FromInstance(_rootGameObject);
            Container.Bind<GameplayHud>().FromInstance(_gameplayHud);
            
            Container.Bind<ISceneFactory>().To<GameplaySceneFactory>().AsSingle();
            Container.Bind<ISceneViewFactory>().To<GameplaySceneViewFactory>().AsSingle();
            
            //ModelsLoader
            Container.Bind<GameplayModelsCreatorService>().AsSingle();
            Container.Bind<GameplayModelsLoaderService>().AsSingle();
            
            //ECS
            Container.Bind<IEcsGameStartUp>().To<EcsGameStartUp>().AsSingle();
        }
    }
}