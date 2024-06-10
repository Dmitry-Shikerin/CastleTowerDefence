using System;
using Sources.BoundedContexts.Bunkers.Domain;
using Sources.BoundedContexts.Bunkers.Presentation.Interfaces;
using Sources.Frameworks.MVPPassiveView.Controllers.Implementation;

namespace Sources.BoundedContexts.Bunkers.Controllers
{
    public class BunkerUiPresenter : PresenterBase
    {
        private readonly Bunker _bunker;
        private readonly IBunkerUi _view;

        public BunkerUiPresenter(Bunker bunker, IBunkerUi view)
        {
            _bunker = bunker ?? throw new ArgumentNullException(nameof(bunker));
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }

        public override void Enable()
        {
            OnHealthChanged();
            _bunker.OnHealthChanged += OnHealthChanged;
        }

        public override void Disable() =>
            _bunker.OnHealthChanged += OnHealthChanged;

        private void OnHealthChanged() =>
            _view.HealthText.SetText(_bunker.Health.ToString());
    }
}