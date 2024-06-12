using Sources.Frameworks.UiFramework.Domain.Commands;

namespace Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ViewCommands.Interfaces
{
    public interface IViewCommand
    {
        FormCommandId Id { get; }

        void Handle();
    }
}