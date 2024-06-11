using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sources.BoundedContexts.BurnAbilities.Domain;
using Sources.BoundedContexts.BurnAbilities.Presentation.Interfaces;
using Sources.Frameworks.MVPPassiveView.Controllers.Implementation;

namespace Sources.BoundedContexts.BurnAbilities.Controllers
{
    public class BurnAbilityPresenter : PresenterBase
    {
        private readonly BurnAbility _burnAbility;
        private readonly IBurnAbilityView _view;

        private CancellationTokenSource _tokenSource;

        public BurnAbilityPresenter(
            BurnAbility burnAbility, 
            IBurnAbilityView view)
        {
            _burnAbility = burnAbility ?? throw new ArgumentNullException(nameof(burnAbility));
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }

        public override void Enable()
        {
            _tokenSource = new CancellationTokenSource();
        }

        public override void Disable()
        {
            _tokenSource.Cancel();
        }

        public void Burn(int instantDamage, int overtimeDamage)
        {
            try
            {
                if (_burnAbility.IsAvailable == false)
                    return;
                
                _tokenSource.Cancel();
                _tokenSource = new CancellationTokenSource();
                
                _view.EnemyHealthView.TakeDamage(instantDamage);
                StartCooldown(_tokenSource.Token);
                StartBurn(_tokenSource.Token, overtimeDamage);
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
                await UniTask.Delay(TimeSpan.FromSeconds(_burnAbility.Cooldown), cancellationToken: cancellationToken);
                _burnAbility.IsAvailable = true;
            }
            catch (OperationCanceledException)
            {
            }
        }
        
        private async void StartBurn(CancellationToken cancellationToken, int overtimeDamage)
        {
            try
            {
                _view.PlayBurnParticle();

                for (int i = 0; i < _burnAbility.BurnTime; i++)
                {
                    _view.EnemyHealthView.TakeDamage(overtimeDamage);

                    await UniTask.Delay(TimeSpan.FromSeconds(
                        1f), cancellationToken: cancellationToken);
                }

                _view.StopBurnParticle();
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}