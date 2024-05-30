using Sources.Frameworks.UiFramework.ButtonCommands.Interfaces.Handlers;
using Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Commands.Implementation;
using Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Commands.Implementation.Handlers;
using Sources.Frameworks.UiFramework.Infrastructure.Commands.Buttons;
using Sources.Frameworks.UiFramework.Infrastructure.Commands.Forms;
using Sources.Frameworks.UiFramework.Infrastructure.Commands.Forms.Handlers;
using Sources.Frameworks.UiFramework.InfrastructureInterfaces.Commands.Views.Handlers;
using Sources.Frameworks.UiFramework.Views.Commands.Implementation;
using Zenject;

namespace Sources.App.DIContainers.Gameplay.UiFarmework
{
    public class GameplayUiFrameworkInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IButtonCommandHandler>().To<GameplayButtonCommandHandler>().AsSingle();

            // Container.Bind<CompleteTutorialCommand>().AsSingle();
            // Container.Bind<LoadMainMenuSceneCommand>().AsSingle();
            // Container.Bind<NewGameCommand>().AsSingle();
            // Container.Bind<LoadGameCommand>().AsSingle();
            // Container.Bind<ShowLeaderboardCommand>().AsSingle();
            // Container.Bind<EnableLoadGameButtonCommand>().AsSingle();
            Container.Bind<UnPauseButtonCommand>().AsSingle();
            // Container.Bind<SetAllMapCameraFollowCommand>().AsSingle();
            // Container.Bind<SetCharacterCameraFollowCommand>().AsSingle();
            Container.Bind<ShowRewardedAdvertisingButtonCommand>().AsSingle();
            // Container.Bind<ClearSavesButtonCommand>().AsSingle();

            Container.Bind<IUiViewCommandHandler>().To<GameplayUiViewCommandHandler>().AsSingle();

            Container.Bind<UnPauseCommand>().AsSingle();
            Container.Bind<PauseCommand>().AsSingle();
            Container.Bind<SaveVolumeCommand>().AsSingle();
            Container.Bind<ClearSavesCommand>().AsSingle();
            // Container.Bind<SetAllMapCameraFollowViewCommand>().AsSingle();
            // Container.Bind<SetCharacterCameraFollowViewCommand>().AsSingle();
        }
    }
}