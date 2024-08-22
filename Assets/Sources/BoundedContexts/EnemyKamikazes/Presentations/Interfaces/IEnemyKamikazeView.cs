using Sources.BoundedContexts.CharacterHealths.PresentationInterfaces;
using Sources.BoundedContexts.CharacterSpawners.Presentation.Interfaces;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Services.Providers;

namespace Sources.BoundedContexts.EnemyKamikazes.Presentations.Interfaces
{
    public interface IEnemyKamikazeView : IEnemyViewBase
    {
        EnemyKamikazeDependencyProvider Provider { get; }
        IEnemyAnimation Animation { get; } 
        ICharacterSpawnPoint CharacterMeleePoint { get; }
        float FindRange { get; }

        void SetTargetFollow(ICharacterHealthView characterViewHealthView);
        void SetCharacterMeleePoint(ICharacterSpawnPoint spawnPoint);
    }
}