using System;
using Sources.Frameworks.UiFramework.Views.Controllers;
using Sources.Frameworks.UiFramework.Views.Infrastructure.Factories.Controllers;
using Sources.Frameworks.UiFramework.Views.Presentations.Implementation;

namespace Sources.Frameworks.UiFramework.Views.Infrastructure.Factories.Views
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