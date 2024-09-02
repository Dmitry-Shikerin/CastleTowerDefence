using System;
using Sources.Frameworks.UiFramework.Core.Services.Forms.Interfaces;
using Sources.Frameworks.UiFramework.Views.Controllers;
using Sources.Frameworks.UiFramework.Views.Presentations.Implementation;
using Sources.Frameworks.UiFramework.Views.Services.Interfaces;

namespace Sources.Frameworks.UiFramework.Views.Infrastructure.Factories.Controllers
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