using System.Collections.Generic;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ViewCommands.Interfaces.Handlers;
using Sources.Frameworks.UiFramework.Views.Domain;
using Sources.Frameworks.UiFramework.Views.Services.Interfaces;

namespace Sources.Frameworks.UiFramework.Views.Services.Implementation
{
    public class UiViewService : IUiViewService
    {
        private readonly IViewCommandHandler _viewCommandHandler;

        public UiViewService(IViewCommandHandler viewCommandHandler)
        {
            _viewCommandHandler = viewCommandHandler;
        }

        public void Handle(IEnumerable<FormCommandId> commandIds)
        {
            foreach (FormCommandId commandId in commandIds)
            {
                _viewCommandHandler.Handle(commandId);
            }
        }
    }
}