using System.Collections.Generic;
using Sources.Frameworks.UiFramework.Domain.Commands;

namespace Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Services.Interfaces
{
    public interface IUiButtonService
    {
        void Handle(IEnumerable<ButtonCommandId> commandIds);
    }
}