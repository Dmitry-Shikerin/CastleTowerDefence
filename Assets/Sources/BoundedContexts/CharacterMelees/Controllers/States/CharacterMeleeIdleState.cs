using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Providers;
using Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces;
using Sources.BoundedContexts.EnemyHealths.Presentation.Implementation;
using Sources.BoundedContexts.EnemyHealths.Presentation.Interfaces;
using Sources.BoundedContexts.Layers.Domain;
using Sources.InfrastructureInterfaces.Services.Overlaps;
using UnityEngine;

namespace Sources.BoundedContexts.CharacterMelees.Controllers.States
{
    [Category("Custom/Character")]
    [UsedImplicitly]
    public class CharacterMeleeIdleState : FSMState
    {
        private ICharacterMeleeView _view;
        private ICharacterMeleeAnimation _animation;
        private IOverlapService _overlapService;
        
        private CancellationTokenSource _cancellationTokenSource;

        protected override void OnInit()
        {
            CharacterMeleeDependencyProvider provider = 
                graphBlackboard.parent.GetVariable<CharacterMeleeDependencyProvider>("_provider").value;

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
                    LayerConst.s_enemy, 
                    LayerConst.s_defaul)
                    .FirstOrDefault();

            if (enemyHealthView != null)
            {
                _view.SetEnemyHealth(enemyHealthView);
            }
        }
    }
}