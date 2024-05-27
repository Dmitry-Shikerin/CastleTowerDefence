using System;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;

namespace Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces
{
    public interface IBossEnemyAnimation : IEnemyAnimation
    {
        event Action ScreamAnimationEnded;
        
        void PlayRun();
    }
}