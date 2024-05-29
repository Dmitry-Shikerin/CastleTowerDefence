using JetBrains.Annotations;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Providers;
using Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces;
using Sources.BoundedContexts.CharacterRanges.Infrastructure.Services.Providers;
using Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces;
using UnityEngine;

namespace Sources.BoundedContexts.CharacterRanges.Controllers.Transitions
{
    [Category("Custom/Character")]
    [UsedImplicitly]
    public class CharacterRangeIdleTransition : ConditionTask
    {
        private ICharacterRangeView _view;

        protected override string OnInit()
        {
            CharacterRangeDependencyProvider provider = 
                blackboard.GetVariable<CharacterRangeDependencyProvider>("_provider").value;
            _view = provider.View;
            return null;
        }

        protected override bool OnCheck() =>
            _view.EnemyHealth == null || Vector3.Distance(
                _view.Position, _view.EnemyHealth.Position) > _view.FindRange;
    }
}