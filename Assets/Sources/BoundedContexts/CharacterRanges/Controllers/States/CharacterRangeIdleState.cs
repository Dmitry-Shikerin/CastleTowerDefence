using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterRanges.Infrastructure.Services.Providers;
using Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces;
using Sources.BoundedContexts.EnemyHealths.Presentation.Implementation;
using Sources.BoundedContexts.EnemyHealths.Presentation.Interfaces;
using Sources.BoundedContexts.Layers.Domain;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;

namespace Sources.BoundedContexts.CharacterRanges.Controllers.States
{
    [Category("Custom/Character")]
    [UsedImplicitly]
    public class CharacterRangeIdleState : FSMState
    {
        private CharacterRangeDependencyProvider _provider;
        
        private ICharacterRangeView View => _provider.View;
        private ICharacterRangeAnimation Animation => _provider.Animation;
        private IOverlapService OverlapService => _provider.OverlapService;
        
        private CancellationTokenSource _cancellationTokenSource;

        protected override void OnInit()
        {
            _provider = 
                graphBlackboard.parent.GetVariable<CharacterRangeDependencyProvider>("_provider").value;
        }

        protected override void OnEnter()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            Animation.PlayIdle();
            StartFind(_cancellationTokenSource.Token);
        }

        protected override void OnUpdate()
        {
            View.SetLookRotation(0);
        }

        protected override void OnExit()
        {
            _cancellationTokenSource.Cancel();
        }
        
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
            IEnemyHealthView enemyHealthView = 
                OverlapService.OverlapSphere<EnemyHealthView>(
                        View.Position, View.FindRange, 
                        LayerConst.Enemy, 
                        LayerConst.Defaul)
                    .FirstOrDefault();

            if(enemyHealthView?.CurrentHealth <= 0)
                return;
            
            if (enemyHealthView != null)
                View.SetEnemyHealth(enemyHealthView);
        }
    }
}