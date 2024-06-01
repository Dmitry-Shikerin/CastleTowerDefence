using Sources.Frameworks.GameServices.Loads.Services.Implementation.Collectors;
using Sources.Infrastructure.Services.LoadServices;
using Sources.Infrastructure.Services.LoadServices.Data;
using Sources.Infrastructure.Services.Repositories;
using Sources.InfrastructureInterfaces.Services.LoadServices;
using Sources.InfrastructureInterfaces.Services.LoadServices.Collectors;
using Sources.InfrastructureInterfaces.Services.LoadServices.Data;
using Sources.InfrastructureInterfaces.Services.Repositories;
using Zenject;

namespace Sources.App.DIContainers.Common
{
    public class SaveLoadServicesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ILoadService>().To<LoadService>().AsSingle();
            Container.Bind<IEntityRepository>().To<EntityRepository>().AsSingle();
            Container.Bind<IDataService>().To<PlayerPrefsDataService>().AsSingle();
            Container.Bind<IMapperCollector>().To<MapperCollector>().AsSingle();
        }
    }
}