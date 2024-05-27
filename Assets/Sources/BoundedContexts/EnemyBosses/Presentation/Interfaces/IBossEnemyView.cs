using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Services.Proveders;

namespace Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces
{
    public interface IBossEnemyView : IEnemyViewBase
    {
        EnemyBossDependencyProvider Provider { get; }
        IBossEnemyAnimation Animation { get; }
        
        void PlayMassAttackParticle();
        void SetAgentSpeed(float speed);
    }
}