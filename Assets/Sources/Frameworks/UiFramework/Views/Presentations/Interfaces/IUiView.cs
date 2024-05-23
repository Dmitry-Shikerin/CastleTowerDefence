using System.Collections.Generic;
using Sources.Frameworks.UiFramework.Presentation.CommonTypes;
using Sources.Frameworks.UiFramework.Presentation.Forms.Types;
using Sources.PresentationsInterfaces.Views;

namespace Sources.Frameworks.UiFramework.PresentationsInterfaces
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
