using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterHealths.Presentation;
using Sources.BoundedContexts.EnemyBosses.Domain;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Services.Providers;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;
using Sources.BoundedContexts.Layers.Domain;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;

namespace Sources.BoundedContexts.EnemyBosses.Controllers.States
{
    [Category("Custom/Enemy")]
    [UsedImplicitly]
    public class EnemyBossMoveToCharacterMeleeState : FSMState
    {
        private BossEnemy _enemy;
        private IEnemyBossView _view;
        private IEnemyBossAnimation _animation;
        private IOverlapService _overlapService;

        private CancellationTokenSource _cancellationTokenSource;

        protected override void OnInit()
        {
            EnemyBossDependencyProvider provider =
                graphBlackboard.parent.GetVariable<EnemyBossDependencyProvider>("_provider").value;

            _enemy = provider.BossEnemy;
            _view = provider.View;
            _animation = provider.Animation;
            _overlapService = provider.OverlapService;
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