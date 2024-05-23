using System;
using Sources.ControllersInterfaces;

namespace Sources.Frameworks.UiFramework.Presentation.Buttons
{
    public class PresentableUiButton<T> : UiButtonSoundView where T : IPresenter
    {
        protected T Presenter { get; private set; }

        protected override void OnAfterEnable()
        {
            Presenter?.Enable();
        }

        protected override void OnAfterDisable()
        {
            Presenter?.Disable();
        }

        public void Construct(T presenter)
        {
            Hide();
            Presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
            Show();
        }

    }
}