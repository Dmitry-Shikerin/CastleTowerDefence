using System.Collections.Generic;
using Sources.Frameworks.UiFramework.Domain.Commands;

namespace Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Signals
{
    public struct HideViewCommandSignal
    {
        public HideViewCommandSignal(IEnumerable<FormCommandId> hideCommands)
        {
            HideCommands = hideCommands;
        }

        public IEnumerable<FormCommandId> HideCommands { get; }
    }
}