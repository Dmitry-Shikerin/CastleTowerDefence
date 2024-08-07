using Sources.Frameworks.UiFramework.Domain.Commands;

namespace Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ViewCommands.Interfaces.Handlers
{
    public interface IViewCommandHandler
    {
        void Handle(FormCommandId formCommandId);
    }
}