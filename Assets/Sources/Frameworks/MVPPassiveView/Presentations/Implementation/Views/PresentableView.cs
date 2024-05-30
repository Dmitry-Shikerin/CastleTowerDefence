using System;
using Sources.Frameworks.MVPPassiveView.Controllers.Interfaces.Presenters;
using Sources.Presentations.Views;

namespace Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views
{
    public class PresentableView<T> : View where T : IPresenter
    {
        private bool _isInitialized;
        
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
            Presenter.Disable();
            Presenter = default;
            _isInitialized = false;
        }

        public void Construct(T presenter)
        {
            Hide();
            Presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
            InitializePresenter();
            Show();
        }

        private void InitializePresenter()
        {
            _isInitialized = true;
            Presenter.Initialize();
        }
    }
}