using Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Services.Interfaces;
using Sources.Frameworks.UiFramework.Collectors;
using Sources.Frameworks.UiFramework.Core.Services.Forms.Implementation;
using Sources.Frameworks.UiFramework.Services.Buttons;
using Zenject;

namespace Sources.App.DIContainers.Common
{
    public class UiFrameworkInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<UiCollectorFactory>().AsSingle();
            Container.Bind<IUiButtonService>().To<UiButtonService>().AsSingle();
            Container.BindInterfacesAndSelfTo<FormService>().AsSingle();
        }
    }
}