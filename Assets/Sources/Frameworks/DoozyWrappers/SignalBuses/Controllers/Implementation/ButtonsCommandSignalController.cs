using System;
using Doozy.Runtime.Signals;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Controllers.Interfaces;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Constants;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Domain;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Signals;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Interfaces.Handlers;

namespace Sources.Frameworks.DoozyWrappers.SignalBuses.Controllers.Implementation
{
    public class ButtonsCommandSignalController : ISignalController
    {
        private readonly IButtonCommandHandler _buttonCommandHandler;

        private SignalReceiver _signalReceiver;
        private SignalStream _signalStream;

        public ButtonsCommandSignalController(
            IButtonCommandHandler buttonCommandHandler)
        {
            _buttonCommandHandler = buttonCommandHandler ??
                                    throw new ArgumentNullException(nameof(buttonCommandHandler));
        }

        public void Initialize()
        {
            _signalReceiver =
                new SignalReceiver()
                    .SetOnSignalCallback(Handle);
            _signalStream = SignalStream
                .Get(StreamConst.ButtonCommand, StreamConst.OnClick)
                .ConnectReceiver(_signalReceiver);
        }

        public void Destroy() =>
            _signalStream.DisconnectReceiver(_signalReceiver);

        private void Handle(Signal signal)
        {
            if (signal.TryGetValue(out ButtonCommandSignal value) == false)
                throw new InvalidOperationException("Signal valueAsObject is not ButtonCommandSignal");

            foreach (ButtonCommandId commandId in value.ButtonCommandIds)
                _buttonCommandHandler.Handle(commandId);
        }
    }
}