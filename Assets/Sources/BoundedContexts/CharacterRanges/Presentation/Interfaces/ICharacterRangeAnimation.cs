using System;

namespace Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces
{
    public interface ICharacterRangeAnimation
    {
        event Action Attacking;
        
        void PlayIdle();
        void PlayAttack();
    }
}