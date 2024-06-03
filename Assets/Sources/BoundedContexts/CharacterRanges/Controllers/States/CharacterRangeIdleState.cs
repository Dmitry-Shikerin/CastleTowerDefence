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
using Sources.InfrastructureInterfaces.Services.Overlaps;
using UnityEngine;

namespace Sources.BoundedContexts.CharacterRanges.Controllers.States
{
    [Category("Custom/Character")]
    [UsedImplicitly]
    public class CharacterRangeIdleState : FSMState
    {
        private ICharacterRangeView _view;
        private ICharacterRangeAnimation _animation;
        private IOverlapService _overlapService;
        
        private CancellationTokenSource _cancellationTokenSource;

        protected override void OnInit()
        {
            CharacterRangeDependencyProvider provider = 
                graphBlackboard.parent.GetVariable<CharacterRangeDependencyProvider>("_provider").value;

            _view = provider.View;
            _animation = provider.Animation;
            _overlapService = provider.OverlapService;
        }

        protected override void OnEnter()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _animation.PlayIdle();
            StartFind(_cancellationTokenSource.Token);
        }

        protected override void OnUpdate()
        {
            _view.SetLookRotation(0);
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
                _overlapService.OverlapSphere<EnemyHealthView>(
                        _view.Position, _view.FindRange, 
                        LayerConst.Enemy, 
                        LayerConst.Defaul)
                    .FirstOrDefault();

            if (enemyHealthView != null)
            {
                _view.SetEnemyHealth(enemyHealthView);
            }
        }
    }
}