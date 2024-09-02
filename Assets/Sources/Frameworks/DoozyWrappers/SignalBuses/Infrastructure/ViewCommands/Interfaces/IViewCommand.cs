using Sources.Frameworks.UiFramework.Views.Domain;

namespace Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ViewCommands.Interfaces
{
    public interface IViewCommand
    {
        FormCommandId Id { get; }

        void Handle();
    }
}