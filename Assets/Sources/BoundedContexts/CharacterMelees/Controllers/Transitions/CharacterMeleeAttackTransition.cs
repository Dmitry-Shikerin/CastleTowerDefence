using JetBrains.Annotations;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Providers;
using Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces;
using UnityEngine;

namespace Sources.BoundedContexts.CharacterMelees.Controllers.Transitions
{
    [Category("Custom/Character")]
    [UsedImplicitly]
    public class CharacterMeleeAttackTransition : ConditionTask
    {
        private ICharacterMeleeView _meleeView;

        protected override string OnInit()
        {
            CharacterMeleeDependencyProvider provider = 
                blackboard.GetVariable<CharacterMeleeDependencyProvider>("_provider").value;
            _meleeView = provider.View;
            
            return null;
        }

        protected override bool OnCheck() =>
            _meleeView.EnemyHealth != null && Vector3.Distance(
                _meleeView.Position, _meleeView.EnemyHealth.Position) < _meleeView.FindRange;
    }
}