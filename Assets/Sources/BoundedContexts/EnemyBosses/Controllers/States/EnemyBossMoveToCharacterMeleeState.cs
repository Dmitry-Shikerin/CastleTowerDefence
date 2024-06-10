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
        private EnemyBossDependencyProvider _provider;
        
        private BossEnemy Enemy => _provider.BossEnemy;
        private IEnemyBossView View => _provider.View;
        private IEnemyBossAnimation Animation => _provider.Animation;
        private IOverlapService OverlapService => _provider.OverlapService;

        private CancellationTokenSource _cancellationTokenSource;

        protected override void OnInit()
        {
            _provider =
                graphBlackboard.parent.GetVariable<EnemyBossDependencyProvider>("_provider").value;
        }

        protected override void OnEnter()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            Animation.PlayWalk();
            StartFind(_cancellationTokenSource.Token);
        }

        protected override void OnUpdate() =>
            View.Move(View.CharacterMeleePoint.Position);

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
                OverlapService.OverlapSphere<CharacterHealthView>(
                        View.Position, View.FindRange,
                        LayerConst.Character,
                        LayerConst.Defaul)
                    .FirstOrDefault();

            if (characterHealthView == null)
                return;

            View.SetCharacterHealth(characterHealthView);
        }
    }
}