using System;
using Doozy.Runtime.Signals;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Controllers.Interfaces;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Constants;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Signals;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ViewCommands.Interfaces.Handlers;
using Sources.Frameworks.UiFramework.Views.Domain;

namespace Sources.Frameworks.DoozyWrappers.SignalBuses.Controllers.Implementation
{
    public class ViewCommandSignalController : ISignalController
    {
        private readonly IViewCommandHandler _viewCommandHandler;

        private SignalReceiver _enableSignalReceiver;
        private SignalStream _enableSignalStream;
        private SignalReceiver _disableSignalReceiver;
        private SignalStream _disableSignalStream;
        

        public ViewCommandSignalController(
            IViewCommandHandler viewCommandHandler)
        {
            _viewCommandHandler = viewCommandHandler ??
                                    throw new ArgumentNullException(nameof(viewCommandHandler));
        }

        public void Initialize()
        {
            _enableSignalReceiver =
                new SignalReceiver()
                    .SetOnSignalCallback(HandleEnable);
            _enableSignalStream = SignalStream
                .Get(StreamConst.ViewCommandCategory, StreamConst.ShowViewCommand)
                .ConnectReceiver(_enableSignalReceiver);
            _disableSignalReceiver =
                new SignalReceiver()
                    .SetOnSignalCallback(HandleDisable);
            _disableSignalStream = SignalStream
                .Get(StreamConst.ViewCommandCategory, StreamConst.HideViewCommand)
                .ConnectReceiver(_disableSignalReceiver);
        }

        public void Destroy()
        {
            _enableSignalStream.DisconnectReceiver(_enableSignalReceiver);
            _disableSignalStream.DisconnectReceiver(_enableSignalReceiver);
        }

        private void HandleEnable(Signal signal)
        {
            if (signal.TryGetValue(out ShowViewCommandSignal value) == false)
                throw new InvalidOperationException("Signal valueAsObject is not ShowViewCommandSignal");

            foreach (FormCommandId commandId in value.ShowCommands)
                _viewCommandHandler.Handle(commandId);
        }
        
        private void HandleDisable(Signal signal)
        {
            if (signal.TryGetValue(out HideViewCommandSignal value) == false)
                throw new InvalidOperationException("Signal valueAsObject is not HideViewCommandSignal");

            foreach (FormCommandId commandId in value.HideCommands)
                _viewCommandHandler.Handle(commandId);
        }
    }
}