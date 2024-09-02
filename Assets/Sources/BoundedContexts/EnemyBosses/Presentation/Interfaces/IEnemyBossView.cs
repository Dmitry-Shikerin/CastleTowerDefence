using Sources.BoundedContexts.Enemies.PresentationInterfaces;

namespace Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces
{
    public interface IEnemyBossView : IEnemyViewBase
    {
        IEnemyBossAnimation Animation { get; }

        float FindRange { get; }

        void PlayMassAttackParticle();
    }
}