using Sources.Frameworks.GameServices.Pauses.Services.Implementation;
using Sources.Frameworks.GameServices.Pauses.Services.Interfaces;
using Sources.Frameworks.GameServices.SceneLoaderServices.Implementation;
using Sources.Frameworks.YandexSdcFramework.Focuses.Interfaces;
using Sources.Frameworks.YandexSdkFramework.Focuses.Implementation;
using Sources.InfrastructureInterfaces.Services.SceneLoaderService;
using Zenject;

namespace Sources.App.DIContainers.Projects
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ISceneLoaderService>().To<SceneLoaderService>().AsSingle();
            Container.Bind<IPauseService>().To<PauseService>().AsSingle();
            Container.Bind<IFocusService>().To<FocusService>().AsSingle();
        }
    }
}