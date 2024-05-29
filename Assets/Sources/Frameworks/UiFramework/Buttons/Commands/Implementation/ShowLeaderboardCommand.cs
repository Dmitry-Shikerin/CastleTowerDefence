using System;
using Doozy.Runtime.Signals;
using Sources.Frameworks.UiFramework.Domain.Commands;
using Sources.Frameworks.UiFramework.InfrastructureInterfaces.Commands.Buttons;
using Sources.Frameworks.UiFramework.Presentation.Forms.Types;
using Sources.Frameworks.UiFramework.PresentationsInterfaces.Buttons;
using Sources.Frameworks.UiFramework.ServicesInterfaces.Forms;
using Sources.Frameworks.YandexSdcFramework.ServicesInterfaces.Leaderboads;
using Sources.Frameworks.YandexSdcFramework.ServicesInterfaces.PlayerAccounts;

namespace Sources.Frameworks.UiFramework.Buttons.Commands.Implementation
{
    public class ShowLeaderboardCommand : IButtonCommand
    {
        private readonly IPlayerAccountAuthorizeService _playerAccountAuthorizeService;
        private readonly ILeaderboardInitializeService _leaderboardInitializeService;
        private readonly IFormService _formService;

        public ShowLeaderboardCommand(
            IPlayerAccountAuthorizeService playerAccountAuthorizeService,
            ILeaderboardInitializeService leaderboardInitializeService,
            IFormService formService)
        {
            _playerAccountAuthorizeService = playerAccountAuthorizeService ?? 
                                             throw new ArgumentNullException(nameof(playerAccountAuthorizeService));
            _leaderboardInitializeService = leaderboardInitializeService ?? 
                                            throw new ArgumentNullException(nameof(leaderboardInitializeService));
            _formService = formService ?? throw new ArgumentNullException(nameof(formService));
        }

        public ButtonCommandId Id => ButtonCommandId.Leaderboard;
        
        public void Handle(IMyUiButton myUiButton)
        {
            SignalStream stream = SignalStream.Get("Leaderboard", "IsAuthorization");
            
            if (_playerAccountAuthorizeService.IsAuthorized() == false)
            {
                stream.SendSignal(false);
                return;
            }

            _leaderboardInitializeService.Fill();
            stream.SendSignal(true);
        }
    }
}