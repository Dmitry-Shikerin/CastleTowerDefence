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
        private CharacterMeleeDependencyProvider _provider;
        
        private ICharacterMeleeView View => _provider.View;

        protected override string OnInit()
        {
            _provider = 
                blackboard.GetVariable<CharacterMeleeDependencyProvider>("_provider").value;
            
            return null;
        }

        protected override bool OnCheck() =>
            View.EnemyHealth != null && Vector3.Distance(
                View.Position, View.EnemyHealth.Position) < View.FindRange;
    }
}