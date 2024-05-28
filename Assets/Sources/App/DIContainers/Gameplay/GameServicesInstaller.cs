using Sources.Frameworks.Services.Linecasts;
using Sources.Frameworks.Services.Linecasts.Interfaces;
using Sources.Frameworks.Services.Overlaps.Implementation;
using Sources.InfrastructureInterfaces.Services.Overlaps;
using Zenject;

namespace Sources.App.DIContainers.Gameplay
{
    public class GameServicesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IOverlapService>().To<OverlapService>().AsSingle();
            Container.Bind<ILinecastService>().To<LinecastService>().AsSingle();
        }
    }
}