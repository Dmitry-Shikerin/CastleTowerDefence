﻿using JetBrains.Annotations;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.EnemyBosses.Domain;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Services.Providers;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;
using UnityEngine;

namespace Sources.BoundedContexts.EnemyBosses.Controllers.Transitions
{
    [Category("Custom/Enemy")]
    [UsedImplicitly]
    public class EnemyBossAttackTransition : ConditionTask
    {
        private EnemyBossDependencyProvider _provider;
        
        private IEnemyBossView View => _provider.View;
        private BossEnemy Enemy => _provider.BossEnemy;

        protected override string OnInit()
        {
            _provider =
                blackboard.GetVariable<EnemyBossDependencyProvider>("_provider").value;

            return null;
        }

        protected override bool OnCheck() =>
            View.CharacterHealthView != null
            && View.CharacterHealthView.CurrentHealth > 0
            && Vector3.Distance(View.Position, View.CharacterHealthView.Position)
            <= View.StoppingDistance + 0.15f;
    }
}