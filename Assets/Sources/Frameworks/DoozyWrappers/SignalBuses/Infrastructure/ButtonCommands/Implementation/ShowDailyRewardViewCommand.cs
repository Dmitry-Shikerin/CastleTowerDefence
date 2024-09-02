using Doozy.Runtime.Signals;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Domain;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Interfaces;

namespace Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Implementation
{
    public class ShowDailyRewardViewCommand : IButtonCommand
    {
        public ButtonCommandId Id => ButtonCommandId.ShowDailyReward;

        public void Handle() =>
            Signal.Send(StreamId.MainMenu.DailyReward, true);
    }
}