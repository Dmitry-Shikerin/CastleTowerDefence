using System;
using Doozy.Runtime.Signals;
using Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Commands.Interfaces;
using Sources.Frameworks.UiFramework.Domain.Commands;
using Sources.Frameworks.YandexSdcFramework.ServicesInterfaces.Leaderboads;
using Sources.Frameworks.YandexSdcFramework.ServicesInterfaces.PlayerAccounts;

namespace Sources.Frameworks.UiFramework.ButtonCommands.Implementation
{
    public class ShowLeaderboardCommand : IButtonCommand
    {
        private readonly IPlayerAccountAuthorizeService _playerAccountAuthorizeService;
        private readonly ILeaderboardInitializeService _leaderboardInitializeService;

        public ShowLeaderboardCommand(
            IPlayerAccountAuthorizeService playerAccountAuthorizeService,
            ILeaderboardInitializeService leaderboardInitializeService)
        {
            _playerAccountAuthorizeService = playerAccountAuthorizeService ??
                                             throw new ArgumentNullException(nameof(playerAccountAuthorizeService));
            _leaderboardInitializeService = leaderboardInitializeService ??
                                            throw new ArgumentNullException(nameof(leaderboardInitializeService));
        }

        public ButtonCommandId Id => ButtonCommandId.Leaderboard;

        public void Handle()
        {
            if (_playerAccountAuthorizeService.IsAuthorized() == false)
            {
                Signal.Send(StreamId.MainMenu.Leaderboard, false);
               
                return;
            }

            _leaderboardInitializeService.Fill();
            Signal.Send(StreamId.MainMenu.Leaderboard, true);
        }
    }
}