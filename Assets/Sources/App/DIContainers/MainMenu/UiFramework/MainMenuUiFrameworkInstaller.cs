using Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Factories.Controllers;
using Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Factories.Views;
using Sources.Frameworks.UiFramework.Buttons.Commands.Implementation;
using Sources.Frameworks.UiFramework.Buttons.Commands.Implementation.Handlers;
using Sources.Frameworks.UiFramework.Buttons.Services.Interfaces;
using Sources.Frameworks.UiFramework.Collectors;
using Sources.Frameworks.UiFramework.Infrastructure.Commands.Buttons;
using Sources.Frameworks.UiFramework.Infrastructure.Commands.Forms;
using Sources.Frameworks.UiFramework.Infrastructure.Commands.Forms.Handlers;
using Sources.Frameworks.UiFramework.InfrastructureInterfaces.Commands.Buttons.Handlers;
using Sources.Frameworks.UiFramework.InfrastructureInterfaces.Commands.Views.Handlers;
using Sources.Frameworks.UiFramework.Services.Buttons;
using Sources.Frameworks.UiFramework.Services.Forms;
using Sources.Frameworks.UiFramework.ServicesInterfaces.Forms;
using Zenject;

namespace Sources.Infrastructure.DIContainers.MainMenu.UiFramework
{
    public class MainMenuUiFrameworkInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<UiCollectorFactory>().AsSingle();
            Container.Bind<ButtonCommandProviderPresenterFactory>().AsSingle();
            Container.Bind<ButtonCommandProviderViewFactory>().AsSingle();
            Container.Bind<IUiButtonService>().To<UiButtonService>().AsSingle();
            Container.BindInterfacesAndSelfTo<FormService>().AsSingle();
            Container.Bind<IButtonCommandHandler>().To<MainMenuButtonCommandHandler>().AsSingle();

            // Container.Bind<ShowFormCommand>().AsSingle();
            // // Container.Bind<CompleteTutorialCommand>().AsSingle();
            // // Container.Bind<LoadMainMenuSceneCommand>().AsSingle();
            // Container.Bind<NewGameCommand>().AsSingle();
            // // Container.Bind<LoadGameCommand>().AsSingle();
             Container.Bind<ShowLeaderboardCommand>().AsSingle();
            // Container.Bind<EnableLoadGameButtonCommand>().AsSingle();
            // Container.Bind<ClearSavesButtonCommand>().AsSingle();
            // // Container.Bind<PlayerAccountAuthorizeButtonCommand>().AsSingle();
            //
            // Container.Bind<IUiViewCommandHandler>().To<MainMenuUiViewCommandHandler>().AsSingle();
            //
            // Container.Bind<UnPauseCommand>().AsSingle();
            // Container.Bind<PauseCommand>().AsSingle();
            // Container.Bind<SaveVolumeCommand>().AsSingle();
            // Container.Bind<ClearSavesCommand>().AsSingle();
        }
    }
}