using Sources.Frameworks.UiFramework.Domain.Commands;
using Sources.Frameworks.UiFramework.PresentationsInterfaces.Buttons;

namespace Sources.Frameworks.UiFramework.InfrastructureInterfaces.Commands.Buttons.Handlers
{
    public interface IButtonCommandHandler
    {
        void Handle(IMyUiButton myUiButton, ButtonCommandId buttonCommandId);
    }
}