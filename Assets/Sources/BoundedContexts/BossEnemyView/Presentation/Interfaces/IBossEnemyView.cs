using Sources.BoundedContexts.BossEnemyView.Infrastructure.Services.Proveders;
using Sources.BoundedContexts.CharacterHealth.PresentationInterfaces;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;

namespace Sources.BoundedContexts.BossEnemyView.Presentation.Interfaces
{
    public interface IBossEnemyView : IEnemyViewBase
    {
        BossEnemyDependencyProvider Provider { get; }
        IBossEnemyAnimation Animation { get; }
        
        void PlayMassAttackParticle();
        void SetAgentSpeed(float speed);
    }
}