using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterHealths.Presentation;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.Layers.Domain;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;
using Sources.Frameworks.Utils.Reflections.Attributes;
using Zenject;

namespace Sources.BoundedContexts.Enemies.Controllers.States
{
    [Category("Custom/Enemy")]
    public class EnemyMoveToBunkerState : FSMState
    {
        private CancellationTokenSource _cancellationTokenSource;
        
        private IEnemyView _view;
        private IEnemyAnimation _animation;
        private IOverlapService _overlapService;

        [Construct]
        private void Construct(EnemyView enemyView)
        {
            _view = enemyView;
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
            var characterHealthView =
                _overlapService.OverlapSphere<CharacterHealthView>(
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