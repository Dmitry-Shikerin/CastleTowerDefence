using System;
using System.Collections.Generic;
using Doozy.Runtime.Signals;
using Sources.Frameworks.GameServices.DoozySignalBuses.Controllers.Actions;
using Sources.Frameworks.GameServices.DoozySignalBuses.Controllers.Interfaces;
using Sources.Frameworks.GameServices.DoozySignalBuses.Domain.Signals.Interfaces;
using Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Commands.Interfaces.Handlers;
using Sources.Frameworks.UiFramework.Domain.Commands;
using Sources.Frameworks.YandexSdcFramework.Leaderboards.Controllers.Actions;

namespace Sources.BoundedContexts.SignalCollectors.Controllers
{
    public class ButtonSignalController : ISignalController
    {
        private readonly IButtonCommandHandler _buttonCommandHandler;
        private readonly List<ISignalAction> _signalActions;
        
        private SignalReceiver _signalReceiver;
        
        public ButtonSignalController(
            ShowLeaderboardSignalAction showLeaderboardSignalAction,
            IButtonCommandHandler buttonCommandHandler)
        {
            _buttonCommandHandler = buttonCommandHandler ?? 
                                    throw new ArgumentNullException(nameof(buttonCommandHandler));
            _signalActions = new List<ISignalAction>
            {
                showLeaderboardSignalAction,
            };
        }

        public void Initialize()
        {
            _signalReceiver = 
                new SignalReceiver()
                .SetOnSignalCallback(Handle);
            SignalStream
                .Get("ButtonCommand", "OnClick")
                .ConnectReceiver(_signalReceiver);
        }

        public void Destroy()
        {
            SignalStream
                .Get("ButtonCommand", "OnClick")
                .DisconnectReceiver(_signalReceiver);
        }

        private void Handle(Signal signal)
        {
            if (signal.TryGetValue(out ButtonCommandSignal value) == false)
                throw new InvalidOperationException("Signal valueAsObject is not ButtonCommandSignal");

            foreach (ButtonCommandId commandId in value.ButtonCommandIds)
                _buttonCommandHandler.Handle(commandId);
        }
    }
}