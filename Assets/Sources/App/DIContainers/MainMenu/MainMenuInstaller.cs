using Sirenix.OdinInspector;
using Sources.BoundedContexts.Huds.Presentations;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Controllers;
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
            Container.Bind<MainMenuSceneViewFactory>().AsSingle();
            Container.Bind<MainMenuSceneFactory>().AsSingle();
        }
    }
}