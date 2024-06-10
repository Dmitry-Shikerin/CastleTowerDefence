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
        private CharacterRangeDependencyProvider _provider;
        
        private ICharacterRangeView View => _provider.View;
        private CharacterRange CharacterRange => _provider.CharacterRange;

        protected override string OnInit()
        {
            _provider =
                blackboard.GetVariable<CharacterRangeDependencyProvider>("_provider").value;

            return null;
        }

        protected override bool OnCheck() =>
            CharacterRange.IsInitialized;
    }
}