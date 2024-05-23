using Sources.Frameworks.UiFramework.Domain.Commands;
using Sources.Frameworks.UiFramework.PresentationsInterfaces.Buttons;

namespace Sources.Frameworks.UiFramework.InfrastructureInterfaces.Commands.Buttons
{
    public interface IButtonCommand
    {
        ButtonCommandId Id { get; }

        void Handle(IUiButton uiButton);
    }
}