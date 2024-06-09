﻿using JetBrains.Annotations;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.EnemyBosses.Domain;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Services.Providers;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;

namespace Sources.BoundedContexts.EnemyBosses.Controllers.States
{
    [Category("Custom/Enemy")]
    [UsedImplicitly]
    public class EnemyBossMoveToCharacterState : FSMState
    {
        private BossEnemy _enemy;
        private IEnemyBossView _view;
        private IEnemyBossAnimation _animation;

        protected override void OnInit()
        {
            EnemyBossDependencyProvider provider = 
                graphBlackboard.parent.GetVariable<EnemyBossDependencyProvider>("_provider").value;

            _enemy = provider.BossEnemy;
            _view = provider.View;
            _animation = provider.Animation;
        }

        protected override void OnEnter()
        {
            base.OnEnter();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
        }

        protected override void OnExit()
        {
            base.OnExit();
        }
    }
}