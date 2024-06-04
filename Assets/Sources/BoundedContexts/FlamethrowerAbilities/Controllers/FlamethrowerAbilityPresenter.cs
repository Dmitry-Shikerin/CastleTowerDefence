using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Sources.BoundedContexts.FlamethrowerAbilities.Domain.Models;
using Sources.BoundedContexts.FlamethrowerAbilities.Presentation.Implementation;
using Sources.BoundedContexts.FlamethrowerAbilities.Presentation.Interfaces;
using Sources.Frameworks.MVPPassiveView.Controllers.Implementation;
using UnityEngine;

namespace Sources.BoundedContexts.FlamethrowerAbilities.Controllers
{
    public class FlamethrowerAbilityPresenter : PresenterBase
    {
        private readonly FlamethrowerAbility _flamethrowerAbility;
        private readonly IFlamethrowerAbilityView _view;

        private CancellationTokenSource _cancellationTokenSource;

        public FlamethrowerAbilityPresenter(FlamethrowerAbility flamethrowerAbility, IFlamethrowerAbilityView view)
        {
            _flamethrowerAbility = flamethrowerAbility ?? throw new ArgumentNullException(nameof(flamethrowerAbility));
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }

        public override void Enable()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _flamethrowerAbility.AbilityApplied += ApplyAbility;
        }

        public override void Disable()
        {
            _cancellationTokenSource.Cancel();
            _flamethrowerAbility.AbilityApplied -= ApplyAbility;
        }

        private async void ApplyAbility()
        {
            try
            {
                IFlamethrowerView view = _view.FlamethrowerView;
                Vector3 from = view.FromPosition;
                Vector3 to = view.ToPosition;
                CancellationToken token = _cancellationTokenSource.Token;
                
                view.SetPosition(from);
                view.Show();
                _view.PlayParticle();
                await MoveAsync(to, token);
                await MoveAsync(from, token);
                _view.StopParticle();
                
            }
            catch (OperationCanceledException)
            {
            }
        }

        private async UniTask MoveAsync(Vector3 target, CancellationToken cancellationToken)
        {
            while (Vector3.Distance(_view.FlamethrowerView.Position, target) > 0.001f)
            {
                _view.FlamethrowerView.Move(target);

                await UniTask.Yield(cancellationToken);
            }
        }
    }
}