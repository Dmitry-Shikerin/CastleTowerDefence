using JetBrains.Annotations;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterMelees.Domain;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Providers;
using Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces;
using UnityEngine;

namespace Sources.BoundedContexts.CharacterMelees.Controllers.Transitions
{
    [Category("Custom/Character")]
    [UsedImplicitly]
    public class CharacterMeleeIdleTransition : ConditionTask
    {
        private ICharacterMeleeView _view;
        private CharacterMelee _characterMelee;

        protected override string OnInit()
        {
            CharacterMeleeDependencyProvider provider = 
                blackboard.GetVariable<CharacterMeleeDependencyProvider>("_provider").value;
            _view = provider.View;
            _characterMelee = provider.CharacterMelee;
            
            return null;
        }

        protected override bool OnCheck() =>
            _view.EnemyHealth == null || Vector3.Distance(
                _view.Position, _view.EnemyHealth.Position) > _view.FindRange;
    }
}