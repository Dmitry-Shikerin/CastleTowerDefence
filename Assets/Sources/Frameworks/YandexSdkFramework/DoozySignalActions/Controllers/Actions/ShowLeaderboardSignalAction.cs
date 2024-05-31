using System;
using Doozy.Runtime.Signals;
using Sources.Frameworks.GameServices.DoozySignalBuses.Controllers.Actions;
using Sources.Frameworks.YandexSdcFramework.ServicesInterfaces.Leaderboads;
using Sources.Frameworks.YandexSdcFramework.ServicesInterfaces.PlayerAccounts;

namespace Sources.Frameworks.YandexSdcFramework.Leaderboards.Controllers.Actions
{
    public class ShowLeaderboardSignalAction : ISignalAction
    {
        private readonly IPlayerAccountAuthorizeService _playerAccountAuthorizeService;
        private readonly ILeaderboardInitializeService _leaderboardInitializeService;

        public ShowLeaderboardSignalAction(
            IPlayerAccountAuthorizeService playerAccountAuthorizeService,
            ILeaderboardInitializeService leaderboardInitializeService)
        {
            _playerAccountAuthorizeService = playerAccountAuthorizeService ??
                                             throw new ArgumentNullException(nameof(playerAccountAuthorizeService));
            _leaderboardInitializeService = leaderboardInitializeService ??
                                            throw new ArgumentNullException(nameof(leaderboardInitializeService));
        }

        public void Handle()
        {
            // SignalStream stream = SignalStream.Get("MainMenu", "Leaderboard");
            // SignalStream stream = SignalStream.Get(, "Leaderboard");
            
            if (_playerAccountAuthorizeService.IsAuthorized() == false)
            {
                // stream.SendSignal(false);
                Signal.Send(StreamId.MainMenu.Leaderboard, false);
               
                return;
            }

            _leaderboardInitializeService.Fill();
            // stream.SendSignal(true);
            Signal.Send(StreamId.MainMenu.Leaderboard, true);
        }
    }
}