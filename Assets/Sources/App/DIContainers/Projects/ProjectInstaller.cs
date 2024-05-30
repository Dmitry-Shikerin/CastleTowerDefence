using Sources.Infrastructure.Services.LoadServices;
using Sources.Infrastructure.Services.SceneLoaderServices;
using Sources.InfrastructureInterfaces.Services.LoadServices;
using Sources.InfrastructureInterfaces.Services.SceneLoaderService;
using Zenject;

namespace Sources.App.DIContainers.Projects
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ISceneLoaderService>().To<SceneLoaderService>().AsSingle();
        }
    }
}