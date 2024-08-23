using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterHealths.Presentation;
using Sources.BoundedContexts.EnemyBosses.Presentation.Implementation;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;
using Sources.BoundedContexts.Layers.Domain;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;
using Sources.Frameworks.Utils.Reflections.Attributes;
using Zenject;

namespace Sources.BoundedContexts.EnemyBosses.Controllers.States
{
    [Category("Custom/Enemy")]
    public class EnemyBossMoveToBunkerState : FSMState
    {
        private IEnemyBossView _view;
        private IEnemyBossAnimation _animation;
        private IOverlapService _overlapService;

        private CancellationTokenSource _cancellationTokenSource;

        [Construct]
        private void Construct(EnemyBossView view)
        {
            _view = view;
            _animation = _view.Animation;
        }

        [Inject]
        private void Construct(IOverlapService overlapService) =>
            _overlapService = overlapService;

        protected override void OnEnter()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _animation.PlayWalk();
            StartFind(_cancellationTokenSource.Token);
        }

        protected override void OnUpdate() =>
            _view.Move(_view.BunkerView.Position);

        protected override void OnExit() =>
            _cancellationTokenSource.Cancel();

        private async void StartFind(CancellationToken cancellationToken)
        {
            try
            {
                while (cancellationToken.IsCancellationRequested == false)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: cancellationToken);
                    FindTarget();
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        private void FindTarget()
        {
            CharacterHealthView characterHealthView =
                _overlapService
                    .OverlapSphere<CharacterHealthView>(
                        _view.Position, _view.FindRange,
                        LayerConst.Character,
                        LayerConst.Defaul)
                    .FirstOrDefault();
            
            if (characterHealthView == null)
                return;
            
            if (characterHealthView.CurrentHealth <= 0)
                return;

            _view.SetCharacterHealth(characterHealthView);
        }
    }
}