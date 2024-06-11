using System.Collections.Generic;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Domain;

namespace Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Signals
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