using Sources.BoundedContexts.CharacterSpawners.Presentation.Interfaces;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Services.Providers;

namespace Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces
{
    public interface IEnemyBossView : IEnemyViewBase
    {
        EnemyBossDependencyProvider Provider { get; }
        IEnemyBossAnimation Animation { get; }
        ICharacterSpawnPoint CharacterMeleePoint { get; set; }
        float FindRange { get; }

        void PlayMassAttackParticle();
        void SetAgentSpeed(float speed);
        void SetCharacterMeleePoint(ICharacterSpawnPoint characterSpawnPoint);
    }
}