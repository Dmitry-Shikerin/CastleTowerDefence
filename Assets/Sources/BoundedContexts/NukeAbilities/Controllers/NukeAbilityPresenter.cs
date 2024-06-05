using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sources.BoundedContexts.EnemyHealths.Presentation.Interfaces;
using Sources.BoundedContexts.NukeAbilities.Domain.Models;
using Sources.BoundedContexts.NukeAbilities.Presentation.Interfaces;
using Sources.Frameworks.MVPPassiveView.Controllers.Implementation;
using UnityEngine;

namespace Sources.BoundedContexts.NukeAbilities.Controllers
{
    public class NukeAbilityPresenter : PresenterBase
    {
        private readonly NukeAbility _nukeAbility;
        private readonly INukeAbilityView _nukeAbilityView;

        private CancellationTokenSource _cancellationTokenSource;

        public NukeAbilityPresenter(NukeAbility nukeAbility, INukeAbilityView nukeAbilityView)
        {
            _nukeAbility = nukeAbility ?? throw new ArgumentNullException(nameof(nukeAbility));
            _nukeAbilityView = nukeAbilityView ?? throw new ArgumentNullException(nameof(nukeAbilityView));
        }

        public override void Enable()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _nukeAbility.AbilityApplied += ApplyAbility;
        }

        public override void Disable()
        {
            _nukeAbility.AbilityApplied -= ApplyAbility;
            _cancellationTokenSource.Cancel();
        }
        
        public void DealDamage(IEnemyHealthView enemyHealthView)
        {
            enemyHealthView.TakeDamage(20);
        }

        private async void ApplyAbility()
        {
            if(_nukeAbility.IsAvailable == false)
                return;
            
            try
            {
                _nukeAbilityView.BombView.SetPosition(_nukeAbilityView.BombView.FromPosition);
                _nukeAbilityView.BombView.Show();
                
                while (Vector3.Distance(_nukeAbilityView.BombView.Position, _nukeAbilityView.BombView.ToPosition) >
                       0.001f)
                {
                    _nukeAbilityView.BombView.Move();

                    await UniTask.Yield(_cancellationTokenSource.Token);
                }
                
                _nukeAbilityView.BombView.Hide();
                _nukeAbilityView.PlayNukeParticle();
            }
            catch (OperationCanceledException)
            {
            }

        }
    }
}