using Sources.Frameworks.GameServices.Loads.Services.Implementation;
using Sources.Frameworks.GameServices.Loads.Services.Implementation.Collectors;
using Sources.Frameworks.GameServices.Loads.Services.Implementation.Data;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces.Collectors;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces.Data;
using Sources.Frameworks.GameServices.Repositories.Services.Implementation;
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