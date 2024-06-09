using System;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;

namespace Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces
{
    public interface IEnemyBossAnimation : IEnemyAnimation
    {
        event Action ScreamAnimationEnded;
        
        void PlayRun();
    }
}