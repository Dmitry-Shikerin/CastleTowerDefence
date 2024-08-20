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
using UnityEngine;

namespace Sources.BoundedContexts.EnemyBosses.Controllers.States
{
    [Category("Custom/Enemy")]
    [UsedImplicitly]
    public class EnemyBossAttackState : FSMState
    {
        private EnemyBossDependencyProvider _provider;
        
        private EnemyAttacker EnemyAttacker => _provider.BossEnemy.EnemyAttacker;
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
            Animation.Attacking += OnAttack;
            Animation.PlayAttack();
            StartTimer(_cancellationTokenSource.Token);
        }
        
        protected override void OnUpdate() =>
            SetCharacterHealth();
        
        protected override void OnExit()
        {
            Animation.Attacking -= OnAttack;
            View.SetCharacterHealth(null);
            _cancellationTokenSource.Cancel();
        }
        
        private void OnAttack()
        {
            SetCharacterHealth();
        
            if (View.CharacterHealthView == null)
                        return;
        
            View.CharacterHealthView.TakeDamage(EnemyAttacker.Damage);
        }
        
        private void SetCharacterHealth()
        {
            if (View.CharacterHealthView == null)
                return;
                    
            if (View.CharacterHealthView.CurrentHealth > 0)
                return;
        
            View.SetCharacterHealth(null);
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
                OverlapService.OverlapSphere<CharacterHealthView>(
                        View.Position, View.FindRange,
                        LayerConst.Character,
                        LayerConst.Defaul);

            View.PlayMassAttackParticle();
            
            if (characterHealthViews.Count == 0)
                return;

            foreach (CharacterHealthView characterHealthView in characterHealthViews)
                characterHealthView.TakeDamage(EnemyAttacker.MassAttackDamage);
        }
    }
}