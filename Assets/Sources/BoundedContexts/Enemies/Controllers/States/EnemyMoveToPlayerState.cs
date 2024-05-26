﻿using JetBrains.Annotations;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.Enemies.Extensions;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Providers;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;

namespace Sources.BoundedContexts.Enemies.Controllers.States
{
    [Category("Custom/Enemy")]
    [UsedImplicitly]
    public class EnemyMoveToPlayerState : FSMState
    {
        private Enemy _enemy;
        private IEnemyView _view;
        private IEnemyAnimation _enemyAnimation;

        protected override void OnInit()
        {
            EnemyDependencyProvider provider = graphBlackboard.GetDependencyProvider<EnemyDependencyProvider>();
            _enemy = provider.Enemy;
            _view = provider.View;
            _enemyAnimation = _view.Animation;
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