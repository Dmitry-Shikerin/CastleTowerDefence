using System.Collections.Generic;
using Sources.Frameworks.UiFramework.Views.Domain;

namespace Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Signals
{
    public struct ShowViewCommandSignal
    {
        public ShowViewCommandSignal(IEnumerable<FormCommandId> showCommands)
        {
            ShowCommands = showCommands;
        }

        public IEnumerable<FormCommandId> ShowCommands { get; }
    }
}