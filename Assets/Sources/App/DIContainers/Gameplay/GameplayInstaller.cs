using Sources.App.Factories;
using Sources.App.Scenes;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views;
using Zenject;

namespace Sources.App.DIContainers.Gameplay
{
    public class GameplayInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GameplaySceneFactory>().AsSingle();
            Container.Bind<GameplaySceneViewFactory>().AsSingle();
        }
    }
}