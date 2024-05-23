using Sources.Frameworks.UiFramework.Domain.Commands;

namespace Sources.Frameworks.UiFramework.InfrastructureInterfaces.Commands
{
    public interface IViewCommand
    {
        FormCommandId Id { get; }

        void Handle();
    }
}