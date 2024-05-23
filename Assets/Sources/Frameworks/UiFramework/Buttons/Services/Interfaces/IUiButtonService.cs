using System.Collections.Generic;
using Sources.Frameworks.UiFramework.Domain.Commands;
using Sources.Frameworks.UiFramework.Presentation.Buttons;
using Sources.Frameworks.UiFramework.PresentationsInterfaces.Buttons;

namespace Sources.Frameworks.UiFramework.Infrastructure.Services.Buttons
{
    public interface IUiButtonService
    {
        void Handle(IEnumerable<ButtonCommandId> commandIds, IUiButton uiButton);
    }
}