using System.Collections.Generic;
using Sources.Frameworks.UiFramework.Domain.Commands;
using Sources.Frameworks.UiFramework.PresentationsInterfaces.Buttons;

namespace Sources.Frameworks.UiFramework.Buttons.Services.Interfaces
{
    public interface IUiButtonService
    {
        void Handle(IEnumerable<ButtonCommandId> commandIds, IMyUiButton myUiButton );
    }
}