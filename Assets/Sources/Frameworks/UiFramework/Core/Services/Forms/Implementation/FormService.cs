using System.Collections.Generic;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using Sources.Frameworks.UiFramework.Core.Services.Forms.Interfaces;
using Sources.Frameworks.UiFramework.Views.Presentations.Implementation;
using Sources.Frameworks.UiFramework.Views.Presentations.Implementation.Types;
using Sources.Frameworks.UiFramework.Views.Presentations.Interfaces;

namespace Sources.Frameworks.UiFramework.Core.Services.Forms.Implementation
{
    public class FormService : IFormService
    {
        private readonly ContainerView _containerView;
        private readonly Dictionary<FormId, IUiView> _forms = new Dictionary<FormId, IUiView>();

        public FormService(UiCollector uiCollector)
        {
        }

        public void Show(FormId formId)
        {
            if (_forms.ContainsKey(formId) == false)
                throw new KeyNotFoundException(nameof(formId));

            _forms[formId].Show();
        }

        public void Hide(FormId formId)
        {
            if (_forms.ContainsKey(formId) == false)
                throw new KeyNotFoundException(nameof(formId));

            if(_forms[formId] == null)
                return;
            
            _forms[formId].Hide();
        }

        public void ShowAll()
        {
            foreach (IUiView form in _forms.Values)
            {
                form.Show();
            }
        }

        public void HideAll()
        {
            foreach (IUiView form in _forms.Values)
            {
                form.Hide();
            }
        }

        public bool IsActive(FormId formId)
        {
            if (_forms.ContainsKey(formId) == false)
                throw new KeyNotFoundException(nameof(formId));

            return _forms[formId].IsActive;
        }
    }
}