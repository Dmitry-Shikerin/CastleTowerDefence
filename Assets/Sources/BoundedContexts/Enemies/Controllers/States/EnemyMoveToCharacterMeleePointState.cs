using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterHealths.Presentation;
using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Providers;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.Layers.Domain;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;

namespace Sources.BoundedContexts.Enemies.Controllers.States
{
    [Category("Custom/Enemy")]
    [UsedImplicitly]
    public class EnemyMoveToCharacterMeleePointState : FSMState
    {
        [RequiredField] public BBParameter<EnemyDependencyProvider> _provider;
        
        private Enemy _enemy;
        private IEnemyView _view;
        private IEnemyAnimation _animation;
        private IOverlapService _overlapService;

        private CancellationTokenSource _cancellationTokenSource;

        protected override void OnInit()
        {
            _enemy = _provider.value.Enemy;
            _view = _provider.value.View;
            _animation = _provider.value.Animation;
            _overlapService = _provider.value.OverlapService;
        }

        protected override void OnEnter()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _animation.PlayWalk();
            StartFind(_cancellationTokenSource.Token);
        }

        protected override void OnUpdate() =>
            _view.Move(_view.CharacterMeleePoint.Position);

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

            _view.SetCharacterHealth(characterHealthView);
        }
    }
}