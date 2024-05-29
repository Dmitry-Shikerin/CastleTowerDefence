using System.Collections.Generic;
using Doozy.Runtime.UIManager.Components;
using Sources.Frameworks.UiFramework.Domain.Commands;

namespace Sources.Frameworks.UiFramework.ButtonProviders.Presentation.Interfaces
{
    public interface IButtonCommandProviderView
    {
        UIButton Button { get; }
        public List<ButtonCommandId> OnClickCommandId { get; }
        public List<ButtonCommandId> EnableCommandId { get; }
        public List<ButtonCommandId> DisableCommandId { get; }
    }
}