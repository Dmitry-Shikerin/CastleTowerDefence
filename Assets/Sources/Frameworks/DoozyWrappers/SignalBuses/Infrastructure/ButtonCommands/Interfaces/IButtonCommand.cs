using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Domain;

namespace Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Interfaces
{
    public interface IButtonCommand
    {
        ButtonCommandId Id { get; }

        void Handle();
    }
}