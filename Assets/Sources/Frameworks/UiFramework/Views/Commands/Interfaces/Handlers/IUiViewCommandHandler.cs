using Sources.Frameworks.UiFramework.Domain.Commands;

namespace Sources.Frameworks.UiFramework.InfrastructureInterfaces.Commands.Views.Handlers
{
    public interface IUiViewCommandHandler
    {
        void Handle(FormCommandId formCommandId);
    }
}