using Sources.Frameworks.UiFramework.Domain.Commands;

namespace Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Commands.Interfaces.Handlers
{
    public interface IButtonCommandHandler
    {
        void Handle(ButtonCommandId buttonCommandId);
    }
}