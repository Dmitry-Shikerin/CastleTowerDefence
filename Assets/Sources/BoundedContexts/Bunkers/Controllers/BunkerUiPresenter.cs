using System;
using Sources.BoundedContexts.Bunkers.Domain;
using Sources.BoundedContexts.Bunkers.Presentation.Interfaces;
using Sources.Frameworks.GameServices.Loads.Domain.Constant;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;
using Sources.Frameworks.MVPPassiveView.Controllers.Implementation;

namespace Sources.BoundedContexts.Bunkers.Controllers
{
    public class BunkerUiPresenter : PresenterBase
    {
        private readonly Bunker _bunker;
        private readonly IBunkerUi _view;

        public BunkerUiPresenter(IEntityRepository entityRepository, IBunkerUi view)
        {
            if (entityRepository == null)
                throw new ArgumentNullException(nameof(entityRepository));
            
            _bunker = entityRepository.Get<Bunker>(ModelId.Bunker);
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }

        public override void Enable()
        {
            HealthChanged();
            _bunker.HealthChanged += HealthChanged;
        }

        public override void Disable() =>
            _bunker.HealthChanged += HealthChanged;

        private void HealthChanged()
        {
            _view.HealthText.SetText(_bunker.Health.ToString());
            _view.HeartAnimator.Play();
        }
    }
}