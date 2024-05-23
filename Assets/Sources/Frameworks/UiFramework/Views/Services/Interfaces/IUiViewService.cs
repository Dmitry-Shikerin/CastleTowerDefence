using System.Collections.Generic;
using Sources.Frameworks.UiFramework.Domain.Commands;

namespace Sources.Frameworks.UiFramework.Infrastructure.Services.Forms
{
    public interface IUiViewService
    {
        void Handle(IEnumerable<FormCommandId> commandIds);
    }
}