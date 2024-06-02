
using System.Collections.Generic;
using Sources.Frameworks.UiFramework.ButtonProviders.Domain;
using Sources.Frameworks.UiFramework.Domain.Commands;

namespace Sources.Frameworks.GameServices.DoozySignalBuses.Domain.Signals.Interfaces
{
    public struct ButtonCommandSignal
    {
        public ButtonCommandSignal(IEnumerable<ButtonCommandId> buttonCommandIds)
        {
            ButtonCommandIds = buttonCommandIds;
        }

        public IEnumerable<ButtonCommandId> ButtonCommandIds { get; }   
    }
}