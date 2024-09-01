using System;
using Sources.Frameworks.MVPPassiveView.Controllers.Interfaces.Presenters;

namespace Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views
{
    public class PresentableView<T> : View where T : IPresenter
    {
        protected T Presenter { get; private set; }

        private void OnEnable()
        {
            Presenter?.Enable();
            OnAfterEnable();
        }

        private void OnDisable()
        {
            OnAfterDisable();
            Presenter?.Disable();
        }

        protected virtual void OnAfterEnable()
        {
        }

        protected virtual void OnAfterDisable()
        {
        }
        
        protected void DestroyPresenter()
        {
            Presenter.Destroy();
            Presenter = default;
        }

        public void Construct(T presenter)
        {
            Hide();
            Presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
            Presenter.Initialize();
            Show();
        }
    }
}