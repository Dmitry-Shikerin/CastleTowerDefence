using System;
using Sources.Frameworks.UiFramework.Controllers.Buttons;
using Sources.Frameworks.UiFramework.Infrastructure.Factories.Controllers.Buttons;
using Sources.Frameworks.UiFramework.Infrastructure.Services.Buttons;
using Sources.Frameworks.UiFramework.Presentation.Buttons;

namespace Sources.Frameworks.UiFramework.Infrastructure.Factories.Views.Buttons
{
    public class UiButtonFactory
    {
        private readonly UiButtonPresenterFactory _presenterFactory;

        public UiButtonFactory(UiButtonPresenterFactory presenterFactory)
        {
            _presenterFactory = presenterFactory ?? 
                                throw new ArgumentNullException(nameof(presenterFactory));
        }

        public UiButton Create(UiButton view)
        {
            UiButtonPresenter presenter = _presenterFactory.Create(view);
            
            view.Construct(presenter);

            return view;
        }
    }
}