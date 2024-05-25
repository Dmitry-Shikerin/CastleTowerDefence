using System;

namespace Sources.BoundedContexts.Enemies.PresentationInterfaces
{
    public interface IEnemyAnimation
    {
        event Action Attacking;
        
        void PlayWalk();
        void PlayIdle();
        void PlayAttack();
    }
}