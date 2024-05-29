using System;

namespace Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces
{
    public interface ICharacterMeleeAnimation
    {
        event Action Attacking;
        
        void PlayIdle();
        void PlayAttack();
    }
}