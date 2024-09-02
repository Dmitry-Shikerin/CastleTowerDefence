using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sources.BoundedContexts.Abilities.Domain;
using Sources.BoundedContexts.Abilities.Presentation.Interfaces;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;
using Sources.Frameworks.MVPPassiveView.Controllers.Implementation;
using UnityEngine;

namespace Sources.BoundedContexts.Abilities.Controllers
{
    public class AbilityApplierPresenter : PresenterBase
    {
        private readonly IAbilityApplier _abilityApplier;
        private readonly IAbilityApplierView _view;
        
        private CancellationTokenSource _tokenSource;
        private TimeSpan _cooldown;

        public AbilityApplierPresenter(
            IEntityRepository entityRepository, 
            string abilityId, 
            IAbilityApplierView view)
        {
            if (entityRepository == null) 
                throw new ArgumentNullException(nameof(entityRepository));
            
            _abilityApplier = entityRepository.Get<IAbilityApplier>(abilityId);
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
            try
            {
                _view.AbilityButton.enabled = false;
                _abilityApplier.IsAvailable = false;
                _view.TimerImage.SetFillAmount(0);

                while (cancellationToken.IsCancellationRequested == false && _view.TimerImage.FillAmount < 1)
                {
                    float fillAmount = Mathf.MoveTowards(
                        _view.TimerImage.FillAmount,
                        1,
                        Time.deltaTime * _abilityApplier.Cooldown);

                    _view.TimerImage.SetFillAmount(fillAmount);

                    await UniTask.Yield(cancellationToken);
                }

                _abilityApplier.IsAvailable = true;
                _view.AbilityButton.enabled = true;
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}