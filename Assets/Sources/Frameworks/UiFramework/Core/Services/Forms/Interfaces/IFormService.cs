using Sources.Frameworks.UiFramework.Views.Presentations.Implementation.Types;

namespace Sources.Frameworks.UiFramework.Core.Services.Forms.Interfaces
{
    public interface IFormService
    {
        void Show(FormId formId);
        void Hide(FormId formId);
        void ShowAll();
        void HideAll();
        bool IsActive(FormId formId);
    }
}
