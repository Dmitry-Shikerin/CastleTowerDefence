using System;
using Sources.Frameworks.UiFramework.Controllers.Forms;
using Sources.Frameworks.UiFramework.Infrastructure.Factories.Controllers.Views;
using Sources.Frameworks.UiFramework.Infrastructure.Services.Forms;
using Sources.Frameworks.UiFramework.Presentation.Forms;
using Sources.Frameworks.UiFramework.Presentation.Views;
using Sources.Frameworks.UiFramework.ServicesInterfaces.Forms;

namespace Sources.Frameworks.UiFramework.Infrastructure.Factories.Views.Forms
{
    public class UiViewFactory
    {
        private readonly UiViewPresenterFactory _presenterFactory;

        public UiViewFactory(UiViewPresenterFactory presenterFactory)
        {
            _presenterFactory = presenterFactory ?? 
                                throw new ArgumentNullException(nameof(presenterFactory));
        }

        public UiView Create(UiView view)
        {
            UiViewPresenter presenter = _presenterFactory.Create(view);
            
            view.Construct(presenter);   
            
            return view;
        }
    }
}