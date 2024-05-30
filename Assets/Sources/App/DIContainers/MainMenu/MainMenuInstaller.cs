using Sirenix.OdinInspector;
using Sources.App.Factories;
using Sources.BoundedContexts.Huds.Presentations;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Controllers.Implementation;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Controllers.Interfaces;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views.Implementation;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views.Interfaces;
using Sources.Frameworks.UiFramework.Views.Presentations.Implementation;
using UnityEngine;
using Zenject;

namespace Sources.App.DIContainers.MainMenu
{
    public class MainMenuInstaller : MonoInstaller
    {
        [Required] [SerializeField] private MainMenuHud _mainMenuHud;
        
        public override void InstallBindings()
        {
            Container.Bind<UiCollector>().FromInstance(_mainMenuHud.UiCollector).AsSingle();
            Container.BindInterfacesAndSelfTo<MainMenuHud>().FromInstance(_mainMenuHud).AsSingle();
            Container.Bind<ISceneViewFactory>().To<MainMenuSceneViewFactory>().AsSingle();
            Container.Bind<ISceneFactory>().To<MainMenuSceneFactory>().AsSingle();
        }
    }
}