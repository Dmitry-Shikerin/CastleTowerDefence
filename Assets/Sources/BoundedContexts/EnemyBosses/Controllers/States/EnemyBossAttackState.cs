using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterHealths.Presentation;
using Sources.BoundedContexts.Enemies.Domain.Models;
using Sources.BoundedContexts.EnemyAttackers.Domain;
using Sources.BoundedContexts.EnemyBosses.Presentation.Implementation;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;
using Sources.BoundedContexts.Layers.Domain;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;
using Sources.Frameworks.Utils.Reflections.Attributes;
using Zenject;

namespace Sources.BoundedContexts.EnemyBosses.Controllers.States
{
    [Category("Custom/Enemy")]
    public class EnemyBossAttackState : FSMState
    {
        private EnemyAttacker _enemyAttacker;
        private IEnemyBossView _view;
        private IEnemyBossAnimation _animation;
        private IOverlapService _overlapService;

        private CancellationTokenSource _cancellationTokenSource;

        [Construct]
        private void Construct(Enemy enemy, EnemyBossView view)
        {
            _enemyAttacker = enemy.EnemyAttacker;
            _view = view;
            _animation = _view.Animation;
        }

        [Inject]
        private void Construct(IOverlapService overlapService) =>
            _overlapService = overlapService;


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
                    await UniTask.Delay(TimeSpan.FromSeconds(4f), cancellationToken: cancellationToken);
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