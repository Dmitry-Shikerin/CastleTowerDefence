using System.Collections.Generic;
using Sources.Frameworks.UiFramework.Domain.Commands;

namespace Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Signals
{
    public class HideViewCommandSignal
    {
        public HideViewCommandSignal(IEnumerable<FormCommandId> hideCommands)
        {
            HideCommands = hideCommands;
        }

        public IEnumerable<FormCommandId> HideCommands { get; }
    }
}