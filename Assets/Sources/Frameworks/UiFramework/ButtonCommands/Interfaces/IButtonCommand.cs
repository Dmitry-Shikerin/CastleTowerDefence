using Sources.Frameworks.UiFramework.Domain.Commands;

namespace Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Commands.Interfaces
{
    public interface IButtonCommand
    {
        ButtonCommandId Id { get; }

        void Handle();
    }
}