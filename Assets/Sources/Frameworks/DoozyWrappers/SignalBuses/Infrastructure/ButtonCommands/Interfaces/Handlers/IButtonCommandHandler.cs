using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Domain;

namespace Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Interfaces.Handlers
{
    public interface IButtonCommandHandler
    {
        void Handle(ButtonCommandId buttonCommandId);
    }
}