using System.Collections.Generic;
using Sources.Frameworks.UiFramework.Domain.Commands;
using Sources.Frameworks.UiFramework.Infrastructure.Commands.Forms.Handlers;
using Sources.Frameworks.UiFramework.InfrastructureInterfaces.Commands.Views.Handlers;

namespace Sources.Frameworks.UiFramework.Infrastructure.Services.Forms
{
    public class UiViewService : IUiViewService
    {
        private readonly IUiViewCommandHandler _uiViewCommandHandler;

        public UiViewService(IUiViewCommandHandler uiViewCommandHandler)
        {
            _uiViewCommandHandler = uiViewCommandHandler;
        }

        public void Handle(IEnumerable<FormCommandId> commandIds)
        {
            foreach (FormCommandId commandId in commandIds)
            {
                _uiViewCommandHandler.Handle(commandId);
            }
        }
    }
}