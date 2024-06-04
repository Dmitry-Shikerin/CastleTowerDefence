using JetBrains.Annotations;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Services.Proveders;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;
using UnityEngine;

namespace Sources.BoundedContexts.EnemyBosses.Controllers.Transitions
{
    [Category("Custom/Enemy")]
    [UsedImplicitly]
    public class EnemyBossMoveToCharacterTransition : ConditionTask
    {
        private IBossEnemyView _view;

        protected override string OnInit()
        {
            EnemyBossDependencyProvider provider = 
                blackboard.GetVariable<EnemyBossDependencyProvider>("_provider").value;
            _view = provider.BossEnemyView;
            
            return null;
        }

        protected override bool OnCheck() =>
            _view.CharacterHealthView != null && 
            Vector3.Distance(_view.Position, _view.CharacterHealthView.Position) > _view.StoppingDistance;
    }
}