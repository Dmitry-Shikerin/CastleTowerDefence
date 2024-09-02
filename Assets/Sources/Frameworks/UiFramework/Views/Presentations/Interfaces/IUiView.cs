using System.Collections.Generic;
using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.Views;
using Sources.Frameworks.UiFramework.Core.Presentation.CommonTypes;
using Sources.Frameworks.UiFramework.Views.Presentations.Implementation.Types;

namespace Sources.Frameworks.UiFramework.Views.Presentations.Interfaces
{
    public interface IUiView : IView
    {
        public Enable EnabledGameObject { get; }
        public Enable EnabledCanvasGroup { get; }
        public ContainerType ContainerType { get; }
        public IReadOnlyList<FormId> OnEnableEnabledForms { get; }
        public IReadOnlyList<FormId> OnEnableDisabledForms { get; }
        
        FormId FormId { get; }
        CustomFormId CustomFormId { get; }
        bool IsActive { get; }
    }
}
