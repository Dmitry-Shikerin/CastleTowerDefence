using System.Collections.Generic;
using Sources.Frameworks.UiFramework.Domain.Commands;

namespace Sources.Frameworks.UiFramework.Views.Services.Interfaces
{
    public interface IUiViewService
    {
        void Handle(IEnumerable<FormCommandId> commandIds);
    }
}