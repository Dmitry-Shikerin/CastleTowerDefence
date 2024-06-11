using System.Collections.Generic;
using Sources.Frameworks.UiFramework.ButtonProviders.Domain;

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