using System;
using Sources.Frameworks.UiFramework.Controllers.Forms;
using Sources.Frameworks.UiFramework.Infrastructure.Services.Forms;
using Sources.Frameworks.UiFramework.Presentation.Forms;
using Sources.Frameworks.UiFramework.Presentation.Views;
using Sources.Frameworks.UiFramework.ServicesInterfaces.Forms;

namespace Sources.Frameworks.UiFramework.Infrastructure.Factories.Controllers.Views
{
    public class UiViewPresenterFactory
    {
        private readonly IUiViewService _uiViewService;
        private readonly IFormService _formService;

        public UiViewPresenterFactory(
            IUiViewService uiViewService, 
            IFormService formService)
        {
            _uiViewService = uiViewService ?? throw new ArgumentNullException(nameof(uiViewService));
            _formService = formService ?? throw new ArgumentNullException(nameof(formService));
        }

        public UiViewPresenter Create(UiView view)
        {
            return new UiViewPresenter(view, _uiViewService, _formService);
        }
    }
}