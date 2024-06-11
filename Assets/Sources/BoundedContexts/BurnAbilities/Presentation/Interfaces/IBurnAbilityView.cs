using Sources.BoundedContexts.EnemyHealths.Presentation.Interfaces;

namespace Sources.BoundedContexts.BurnAbilities.Presentation.Interfaces
{
    public interface IBurnAbilityView : IBurnable
    {
        IEnemyHealthView EnemyHealthView { get; }
        
        void PlayBurnParticle();
        void StopBurnParticle();
    }
}