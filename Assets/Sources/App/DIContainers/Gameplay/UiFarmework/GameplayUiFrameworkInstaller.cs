using Sources.Frameworks.UiFramework.Buttons.Commands.Implementation;
using Sources.Frameworks.UiFramework.Buttons.Commands.Implementation.Handlers;
using Sources.Frameworks.UiFramework.Infrastructure.Commands.Buttons;
using Sources.Frameworks.UiFramework.Infrastructure.Commands.Forms;
using Sources.Frameworks.UiFramework.Infrastructure.Commands.Forms.Handlers;
using Sources.Frameworks.UiFramework.InfrastructureInterfaces.Commands.Buttons.Handlers;
using Sources.Frameworks.UiFramework.InfrastructureInterfaces.Commands.Views.Handlers;
using Zenject;

namespace Sources.Infrastructure.DIContainers.Gameplay.UiFramework
{
    public class GameplayUiFrameworkInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IButtonCommandHandler>().To<GameplayButtonCommandHandler>().AsSingle();

            Container.Bind<ShowFormCommand>().AsSingle();
            // Container.Bind<CompleteTutorialCommand>().AsSingle();
            // Container.Bind<LoadMainMenuSceneCommand>().AsSingle();
            Container.Bind<NewGameCommand>().AsSingle();
            // Container.Bind<LoadGameCommand>().AsSingle();
            Container.Bind<ShowLeaderboardCommand>().AsSingle();
            Container.Bind<EnableLoadGameButtonCommand>().AsSingle();
            Container.Bind<UnPauseButtonCommand>().AsSingle();
            Container.Bind<HideFormCommand>().AsSingle();
            // Container.Bind<SetAllMapCameraFollowCommand>().AsSingle();
            // Container.Bind<SetCharacterCameraFollowCommand>().AsSingle();
            Container.Bind<ShowRewardedAdvertisingButtonCommand>().AsSingle();
            Container.Bind<ClearSavesButtonCommand>().AsSingle();

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