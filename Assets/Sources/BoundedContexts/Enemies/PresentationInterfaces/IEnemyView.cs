using NodeCanvas.StateMachines;
using Sources.BoundedContexts.CharacterSpawners.Presentation.Interfaces;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Providers;
using Sources.BoundedContexts.SpawnPoints.Presentation.Interfaces;

namespace Sources.BoundedContexts.Enemies.PresentationInterfaces
{
    public interface IEnemyView : IEnemyViewBase
    {
        IEnemyAnimation Animation { get; }
        EnemyDependencyProvider Provider { get; }
        ICharacterSpawnPoint CharacterMeleePoint { get; }
        float FindRange { get; }

        void SetCharacterMeleePoint(ICharacterSpawnPoint spawnPoint);
    }
}