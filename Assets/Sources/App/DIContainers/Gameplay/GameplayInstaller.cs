using Sirenix.OdinInspector;
using Sources.App.Factories;
using Sources.BoundedContexts.RootGameObjects.Presentation;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Controllers.Implementation;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Controllers.Interfaces;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views.Implementation;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views.Interfaces;
using UnityEngine;
using Zenject;

namespace Sources.App.DIContainers.Gameplay
{
    public class GameplayInstaller : MonoInstaller
    {
        [Required] [SerializeField] private RootGameObject _rootGameObject;
        
        public override void InstallBindings()
        {
            Container.Bind<RootGameObject>().FromInstance(_rootGameObject);
            
            Container.Bind<ISceneFactory>().To<GameplaySceneFactory>().AsSingle();
            Container.Bind<ISceneViewFactory>().To<GameplaySceneViewFactory>().AsSingle();
        }
    }
}