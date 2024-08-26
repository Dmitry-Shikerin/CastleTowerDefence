using System;
using Doozy.Runtime.Signals;
using Sources.BoundedContexts.Bunkers.Domain;
using Sources.BoundedContexts.Bunkers.Presentation.Interfaces;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Constants;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Signals;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;
using Sources.Frameworks.MVPPassiveView.Controllers.Implementation;
using Sources.Frameworks.UiFramework.AudioSources.Presentations.Implementation.Types;

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