using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sources.BoundedContexts.Abilities.Domain;
using Sources.BoundedContexts.Abilities.Presentation.Interfaces;
using Sources.Frameworks.MVPPassiveView.Controllers.Implementation;

namespace Sources.BoundedContexts.Abilities.Controllers
{
    public class AbilityApplierPresenter : PresenterBase
    {
        private readonly IAbilityApplier _abilityApplier;
        private readonly IAbilityApplierView _view;
        
        private CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private TimeSpan _cooldown;

        public AbilityApplierPresenter(IAbilityApplier abilityApplier, IAbilityApplierView view)
        {
            _abilityApplier = abilityApplier ?? throw new ArgumentNullException(nameof(abilityApplier));
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }

        public override void Enable()
        {
            _tokenSource = new CancellationTokenSource();
            _view.AbilityButton.onClickEvent.AddListener(ApplyAbility);
            _cooldown = TimeSpan.FromSeconds(_abilityApplier.Cooldown);
        }

        public override void Disable()
        {
            _view.AbilityButton.onClickEvent.RemoveListener(ApplyAbility);
            _tokenSource.Cancel();
        }

        private void ApplyAbility()
        {
            if(_abilityApplier.IsAvailable == false)
                return;
            
            _abilityApplier.ApplyAbility();
            StartTimer(_tokenSource.Token);
        }

        private async void StartTimer(CancellationToken cancellationToken)
        {
            _view.AbilityButton.enabled = false;
            _abilityApplier.IsAvailable = false;
            await UniTask.Delay(_cooldown, cancellationToken: cancellationToken);
            _abilityApplier.IsAvailable = true;
            _view.AbilityButton.enabled = true;
        }
    }
}