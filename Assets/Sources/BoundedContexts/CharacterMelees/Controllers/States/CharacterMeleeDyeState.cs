using JetBrains.Annotations;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Providers;
using Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces;

namespace Sources.BoundedContexts.CharacterMelees.Controllers.States
{
    [Category("Custom/Character")]
    [UsedImplicitly]
    public class CharacterMeleeDyeState : FSMState
    {
        private CharacterMeleeDependencyProvider _provider;
        private ICharacterMeleeView View => _provider.View;

        protected override void OnInit()
        {
            _provider = 
                graphBlackboard.parent.GetVariable<CharacterMeleeDependencyProvider>("_provider").value;
        }

        protected override void OnEnter()
        {
            View.Destroy();
        }
    }
}