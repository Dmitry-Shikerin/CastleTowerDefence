using Sources.Frameworks.UiFramework.Domain.Commands;

namespace Sources.Frameworks.UiFramework.ButtonCommands.Interfaces.Handlers
{
    public interface IButtonCommandHandler
    {
        void Handle(ButtonCommandId buttonCommandId);
    }
}