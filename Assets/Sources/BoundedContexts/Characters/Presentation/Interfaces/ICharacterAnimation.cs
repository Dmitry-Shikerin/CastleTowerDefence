using System;

namespace Sources.BoundedContexts.Characters.Presentation.Interfaces
{
    public interface ICharacterAnimation
    {
        event Action Attacking;
        
        void PlayIdle();
        void PlayAttack();
    }
}