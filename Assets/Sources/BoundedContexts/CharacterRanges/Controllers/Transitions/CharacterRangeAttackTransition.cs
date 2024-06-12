using JetBrains.Annotations;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterRanges.Infrastructure.Services.Providers;
using Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces;
using UnityEngine;

namespace Sources.BoundedContexts.CharacterRanges.Controllers.Transitions
{
    [Category("Custom/Character")]
    [UsedImplicitly]
    public class CharacterRangeAttackTransition : ConditionTask
    {
        private CharacterRangeDependencyProvider _provider;
        
        private ICharacterRangeView View => _provider.View;

        protected override string OnInit()
        {
            _provider =
                blackboard.GetVariable<CharacterRangeDependencyProvider>("_provider").value;
            
            return null;
        }

        protected override bool OnCheck() =>
            View.EnemyHealth != null && Vector3.Distance(
                View.Position, View.EnemyHealth.Position) < View.FindRange;
    }
}