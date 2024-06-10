using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterHealths.Presentation;
using Sources.BoundedContexts.EnemyAttackers.Domain;
using Sources.BoundedContexts.EnemyBosses.Domain;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Services.Providers;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;
using Sources.BoundedContexts.Layers.Domain;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;

namespace Sources.BoundedContexts.EnemyBosses.Controllers.States
{
    [Category("Custom/Enemy")]
    [UsedImplicitly]
    public class EnemyBossAttackState : FSMState
    {
        private BossEnemy _enemy;
        private EnemyAttacker _enemyAttacker;
        private IEnemyBossView _view;
        private IEnemyBossAnimation _animation;
        private IOverlapService _overlapService;

        private CancellationTokenSource _cancellationTokenSource;

        protected override void OnInit()
        {
            EnemyBossDependencyProvider provider =
                graphBlackboard.parent.GetVariable<EnemyBossDependencyProvider>("_provider").value;
        
            _enemy = provider.BossEnemy;
            _enemyAttacker = _enemy.EnemyAttacker;
            _view = provider.View;
            _animation = provider.Animation;
            _overlapService = provider.OverlapService;
        }
        
        protected override void OnEnter()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _animation.Attacking += OnAttack;
            _animation.PlayAttack();
            StartTimer(_cancellationTokenSource.Token);
        }
        
        protected override void OnUpdate() =>
            SetCharacterHealth();
        
        protected override void OnExit()
        {
            _animation.Attacking -= OnAttack;
            _view.SetCharacterHealth(null);
            _cancellationTokenSource.Cancel();
        }
        
        private void OnAttack()
        {
            SetCharacterHealth();
        
            if (_view.CharacterHealthView == null)
                        return;
        
            _view.CharacterHealthView.TakeDamage(_enemyAttacker.Damage);
        }
        
        private void SetCharacterHealth()
        {
            if (_view.CharacterHealthView == null)
                return;
                    
            if (_view.CharacterHealthView.CurrentHealth > 0)
                return;
        
            _view.SetCharacterHealth(null);
        }
        
        private async void StartTimer(CancellationToken cancellationToken)
        {
            try
            {
                while (cancellationToken.IsCancellationRequested == false)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(5f), cancellationToken: cancellationToken);
                    PlayMassAttack();
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        private void PlayMassAttack()
        {
            IReadOnlyList<CharacterHealthView> characterHealthViews =
                _overlapService.OverlapSphere<CharacterHealthView>(
                        _view.Position, _view.FindRange,
                        LayerConst.Character,
                        LayerConst.Defaul);

            _view.PlayMassAttackParticle();
            
            if (characterHealthViews.Count == 0)
                return;

            foreach (CharacterHealthView characterHealthView in characterHealthViews)
                characterHealthView.TakeDamage(_enemyAttacker.MassAttackDamage);
        }
    }
}