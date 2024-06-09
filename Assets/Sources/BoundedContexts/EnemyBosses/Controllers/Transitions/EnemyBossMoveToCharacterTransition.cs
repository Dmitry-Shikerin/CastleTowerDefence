using JetBrains.Annotations;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Services.Providers;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;
using UnityEngine;

namespace Sources.BoundedContexts.EnemyBosses.Controllers.Transitions
{
    [Category("Custom/Enemy")]
    [UsedImplicitly]
    public class EnemyBossMoveToCharacterTransition : ConditionTask
    {
        private IEnemyBossView _view;

        protected override string OnInit()
        {
            EnemyBossDependencyProvider provider = 
                blackboard.GetVariable<EnemyBossDependencyProvider>("_provider").value;
            _view = provider.View;
            
            return null;
        }

        protected override bool OnCheck() =>
            _view.CharacterHealthView != null && _view.CharacterHealthView.CurrentHealth > 0;
    }
}