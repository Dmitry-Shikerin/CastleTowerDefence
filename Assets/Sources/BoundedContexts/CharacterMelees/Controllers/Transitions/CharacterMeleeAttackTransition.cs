using JetBrains.Annotations;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Providers;
using Sources.BoundedContexts.CharacterMelees.PresentationInterfaces;

namespace Sources.BoundedContexts.Characters.Controllers.Transitions
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
            _meleeView = provider.MeleeView;
            return null;
        }

        protected override bool OnCheck() =>
            _meleeView.EnemyHealthView != null;
    }
}