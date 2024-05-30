using Sources.Frameworks.UiFramework.ButtonCommands.Implementation;
using Sources.Frameworks.UiFramework.ButtonCommands.Implementation.Handlers;
using Sources.Frameworks.UiFramework.ButtonCommands.Interfaces.Handlers;
using Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Commands.Implementation;
using Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Commands.Implementation.Handlers;
using Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Services.Interfaces;
using Sources.Frameworks.UiFramework.Collectors;
using Sources.Frameworks.UiFramework.Core.Services.Forms.Implementation;
using Sources.Frameworks.UiFramework.Services.Buttons;
using Zenject;

namespace Sources.App.DIContainers.MainMenu.UiFramework
{
    public class MainMenuUiFrameworkInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IButtonCommandHandler>().To<MainMenuButtonCommandHandler>().AsSingle();

            // // Container.Bind<CompleteTutorialCommand>().AsSingle();
            Container.Bind<NewGameCommand>().AsSingle();
            // Container.Bind<LoadGameCommand>().AsSingle();
            Container.Bind<ShowLeaderboardCommand>().AsSingle();
            // Container.Bind<EnableLoadGameButtonCommand>().AsSingle();
            // Container.Bind<ClearSavesButtonCommand>().AsSingle();
            // Container.Bind<PlayerAccountAuthorizeButtonCommand>().AsSingle();
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