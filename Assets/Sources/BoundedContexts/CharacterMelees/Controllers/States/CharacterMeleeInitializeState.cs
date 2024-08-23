using System;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterMelees.Domain;
using Sources.BoundedContexts.CharacterMelees.Presentation.Implementation;
using Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces;
using Sources.Frameworks.Utils.Reflections.Attributes;

namespace Sources.BoundedContexts.CharacterMelees.Controllers.States
{
    [Category("Custom/Character")]
    public class CharacterMeleeInitializeState : FSMState
    {
        private ICharacterMeleeView _view;
        private ICharacterMeleeAnimation _animation;
        private CharacterMelee _characterMelee;

        [Construct]
        private void Construct(CharacterMelee characterMelee, CharacterMeleeView view)
        {
            _characterMelee = characterMelee ?? throw new ArgumentNullException(nameof(characterMelee));
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _animation = view.MeleeAnimation;
        }

        protected override void OnEnter()
        {
            _animation.PlayIdle();
            _characterMelee.IsInitialized = true;
            _view.CharacterSpawnPoint.SetCharacterHealth(_view.HealthView);
        }
    }
}