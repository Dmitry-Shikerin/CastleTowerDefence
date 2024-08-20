using Sirenix.OdinInspector;
using Sources.BoundedContexts.Tutorials.Services.Implementation;
using Sources.BoundedContexts.Tutorials.Services.Interfaces;
using Sources.Frameworks.GameServices.Addressables.Interfaces;
using Sources.Frameworks.GameServices.Cameras.Infrastructure.Services.Implementation;
using Sources.Frameworks.GameServices.Cameras.Presentation.Implementation;
using Sources.Frameworks.GameServices.ObjectPools.Implementation.Managers;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Managers;
using Sources.Frameworks.GameServices.Overlaps.Implementation;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;
using Sources.Frameworks.GameServices.Prefabs.Implementation;
using Sources.Frameworks.GameServices.Prefabs.Implementation.Composites;
using Sources.Frameworks.GameServices.Prefabs.Interfaces;
using Sources.Frameworks.MyGameCreator.SkyAndWeathers.Infrastructure.Services.Implementation;
using Sources.Frameworks.MyGameCreator.SkyAndWeathers.Presentation;
using Sources.Frameworks.Services.Linecasts;
using Sources.Frameworks.Services.Linecasts.Interfaces;
using Sources.InfrastructureInterfaces.Services.Cameras;
using UnityEngine;
using Zenject;

namespace Sources.App.DIContainers.Gameplay
{
    public class GameServicesInstaller : MonoInstaller
    {
        [Required] [SerializeField] private CameraView _cameraView;
        [Required] [SerializeField] private SkyAndWeatherView _skyAndWeatherView;
        
        public override void InstallBindings()
        {
            Container.Bind<ITutorialService>().To<TutorialService>().AsSingle();
            Container.Bind<IOverlapService>().To<OverlapService>().AsSingle();
            Container.Bind<ILinecastService>().To<LinecastService>().AsSingle();
            Container.Bind<IPrefabCollector>().To<PrefabCollector>().AsSingle();
            Container.Bind<IResourcesAssetLoader>().To<ResourcesAssetLoader>().AsSingle();
            Container.Bind<IPoolManager>().To<PoolManager>().AsSingle();
            
            //Camera
            Container.Bind<CameraView>().FromInstance(_cameraView).AsSingle();
            Container.Bind<ICameraService>().To<CameraService>().AsSingle();
            
            //SkyAndWeather
            Container.Bind<SkyAndWeatherView>().FromInstance(_skyAndWeatherView).AsSingle();
            Container.Bind<ISkyAndWeatherService>().To<SkyAndWeatherService>().AsSingle();
            
            //Addressables
            Container.Bind<IAddressablesAssetLoader>().To<AddressablesAssetLoader>().AsSingle();
            Container.Bind<ICompositeAssetService>().To<CompositeAssetService>().AsSingle();
        }
    }
}