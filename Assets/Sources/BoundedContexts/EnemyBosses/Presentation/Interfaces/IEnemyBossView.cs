using Sources.BoundedContexts.CharacterSpawners.Presentation.Interfaces;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;

namespace Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces
{
    public interface IEnemyBossView : IEnemyViewBase
    {
        IEnemyBossAnimation Animation { get; }
        ICharacterSpawnPoint CharacterMeleePoint { get; set; }
        float FindRange { get; }

        void PlayMassAttackParticle();
        void SetAgentSpeed(float speed);
        void SetCharacterMeleePoint(ICharacterSpawnPoint characterSpawnPoint);
    }
}