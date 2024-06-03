using JetBrains.Annotations;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterRanges.Domain;
using Sources.BoundedContexts.CharacterRanges.Infrastructure.Services.Providers;
using Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces;

namespace Sources.BoundedContexts.CharacterRanges.Controllers.Transitions
{
    [Category("Custom/Character")]
    [UsedImplicitly]
    public class FromInitializedToCharacterRangeIdleTransition : ConditionTask
    {
        private ICharacterRangeView _view;

        private CharacterRange _characterMelee;

        protected override string OnInit()
        {
            CharacterRangeDependencyProvider provider =
                blackboard.GetVariable<CharacterRangeDependencyProvider>("_provider").value;
            _view = provider.View;
            _characterMelee = provider.CharacterRange;

            return null;
        }

        protected override bool OnCheck() =>
            _characterMelee.IsInitialized;
    }
}