using System;
using Sources.BoundedContexts.Healths.DomainInterfaces;
using Sources.BoundedContexts.Healths.Presentation.Interfaces;
using Sources.Frameworks.Domain.Implementation.Constants;
using Sources.Frameworks.MVPPassiveView.Controllers.Implementation;
using Sources.Frameworks.Utils.Extensions;

namespace Sources.BoundedContexts.Healths.Controllers
{
    public class HealthBarPresenter : PresenterBase
    {
        private readonly IHealth _health;
        private readonly IHealthBarView _view;

        public HealthBarPresenter(IHealth health, IHealthBarView view)
        {
            _health = health ?? throw new ArgumentNullException(nameof(health));
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }

        public override void Enable()
        {
            OnHealthChanged();
            _health.HealthChanged += OnHealthChanged;
        }

        public override void Disable() =>
            _health.HealthChanged -= OnHealthChanged;

        private void OnHealthChanged()
        {
            float percent = _health.CurrentHealth.FloatToPercent(_health.MaxHealth);
            float fillAmount = percent * MathConst.UnitMultiplier;

            _view.HealthBarImage.SetFillAmount(fillAmount);
        }
    }
}