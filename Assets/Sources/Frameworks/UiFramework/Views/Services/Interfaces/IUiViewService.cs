using System.Collections.Generic;
using Sources.Frameworks.UiFramework.Views.Domain;

namespace Sources.Frameworks.UiFramework.Views.Services.Interfaces
{
    public interface IUiViewService
    {
        void Handle(IEnumerable<FormCommandId> commandIds);
    }
}