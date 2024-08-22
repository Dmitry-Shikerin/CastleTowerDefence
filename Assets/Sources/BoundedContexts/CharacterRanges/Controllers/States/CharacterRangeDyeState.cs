using JetBrains.Annotations;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterRanges.Infrastructure.Services.Providers;
using Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces;
using Sources.BoundedContexts.CharacterRotations.Services.Interfaces;

namespace Sources.BoundedContexts.CharacterRanges.Controllers.States
{
    [Category("Custom/Character")]
    [UsedImplicitly]
    public class CharacterRangeDyeState : FSMState
    {
        private CharacterRangeDependencyProvider _provider;
        
        private ICharacterRangeView View => _provider.View;
        private ICharacterRangeAnimation Animation => _provider.Animation;
        private ICharacterRotationService RotationService => _provider.CharacterRotationService;

        protected override void OnInit()
        {
            _provider = 
                graphBlackboard.parent.GetVariable<CharacterRangeDependencyProvider>("_provider").value;
        }

        protected override void OnEnter()
        {
            View.Destroy();
        }
    }
}