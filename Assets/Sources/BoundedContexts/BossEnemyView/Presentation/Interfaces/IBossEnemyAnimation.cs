using System;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;

namespace Sources.BoundedContexts.BossEnemyView.Presentation.Interfaces
{
    public interface IBossEnemyAnimation : IEnemyAnimation
    {
        event Action ScreamAnimationEnded;
        
        void PlayRun();
    }
}