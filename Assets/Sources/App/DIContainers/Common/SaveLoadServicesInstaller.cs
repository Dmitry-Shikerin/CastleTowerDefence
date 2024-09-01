using Sources.Frameworks.GameServices.Loads.Services.Implementation;
using Sources.Frameworks.GameServices.Loads.Services.Implementation.Data;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces.Data;
using Sources.Frameworks.GameServices.Prefabs.Implementation;
using Sources.Frameworks.GameServices.Prefabs.Implementation.Composites;
using Sources.Frameworks.GameServices.Prefabs.Interfaces;
using Sources.Frameworks.GameServices.Prefabs.Interfaces.Composites;
using Sources.Frameworks.GameServices.Repositories.Services.Implementation;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;
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
            // Container.Bind<IDataService>().To<EasySaveDataService>().AsSingle();
            
            //Assets
            Container.Bind<IAssetCollector>().To<AssetCollector>().AsSingle();
            Container.Bind<IResourcesAssetLoader>().To<ResourcesAssetLoader>().AsSingle();
            Container.Bind<IAddressablesAssetLoader>().To<AddressablesAssetLoader>().AsSingle();
            Container.Bind<ICompositeAssetService>().To<CompositeAssetService>().AsSingle();
        }
    }
}