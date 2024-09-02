using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sources.BoundedContexts.BurnAbilities.Domain;
using Sources.BoundedContexts.BurnAbilities.Presentation.Interfaces;
using Sources.BoundedContexts.FlamethrowerAbilities.Domain.Constants;
using Sources.Frameworks.MVPPassiveView.Controllers.Implementation;

namespace Sources.BoundedContexts.BurnAbilities.Controllers
{
    public class BurnAbilityPresenter : PresenterBase
    {
        private readonly BurnAbility _burnAbility;
        private readonly IBurnAbilityView _view;

        private CancellationTokenSource _tokenSource;
        private readonly TimeSpan _burnDelay;
        private readonly TimeSpan _abilityCooldown;

        public BurnAbilityPresenter(
            BurnAbility burnAbility, 
            IBurnAbilityView view)
        {
            _burnAbility = burnAbility ?? throw new ArgumentNullException(nameof(burnAbility));
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _burnDelay = TimeSpan.FromSeconds(1f);
            _abilityCooldown = TimeSpan.FromSeconds(_burnAbility.Cooldown);
        }

        public override void Enable() =>
            _tokenSource = new CancellationTokenSource();

        public override void Disable() =>
            _tokenSource.Cancel();

        public void Burn()
        {
            try
            {
                if (_burnAbility.IsAvailable == false)
                    return;
                
                _tokenSource.Cancel();
                _tokenSource = new CancellationTokenSource();
                
                _view.EnemyHealthView.TakeDamage(_view.EnemyHealthView.MaxHealth * FlamethrowerConts.InstantDamage);
                StartCooldown(_tokenSource.Token);
                StartBurn(_tokenSource.Token);
            }
            catch (OperationCanceledException)
            {
            }
        }

        private async void StartCooldown(CancellationToken cancellationToken)
        {
            try
            {
                _burnAbility.IsAvailable = false;
                await UniTask.Delay(_abilityCooldown, cancellationToken: cancellationToken);
                _burnAbility.IsAvailable = true;
            }
            catch (OperationCanceledException)
            {
            }
        }
        
        private async void StartBurn(CancellationToken cancellationToken)
        {
            try
            {
                _view.PlayBurnParticle();

                for (int i = 0; i < _burnAbility.BurnTick; i++)
                {
                    _view.EnemyHealthView.TakeDamage(_view.EnemyHealthView.MaxHealth * FlamethrowerConts.OvertimeDamage);
                    await UniTask.Delay(_burnDelay, cancellationToken: cancellationToken);
                }

                _view.StopBurnParticle();
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}