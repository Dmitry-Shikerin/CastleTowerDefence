using Sirenix.OdinInspector;
using Sources.App.Factories;
using Sources.App.Scenes;
using Sources.BoundedContexts.RootGameObjects.Presentation;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views;
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
            
            Container.Bind<GameplaySceneFactory>().AsSingle();
            Container.Bind<GameplaySceneViewFactory>().AsSingle();
        }
    }
}