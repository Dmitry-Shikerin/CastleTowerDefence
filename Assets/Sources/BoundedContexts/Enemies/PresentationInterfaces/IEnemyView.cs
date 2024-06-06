using NodeCanvas.StateMachines;
using Sources.BoundedContexts.CharacterHealth.PresentationInterfaces;
using Sources.BoundedContexts.CharacterSpawners.Presentation.Interfaces;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Providers;
using Sources.BoundedContexts.SpawnPoints.Presentation.Interfaces;

namespace Sources.BoundedContexts.Enemies.PresentationInterfaces
{
    public interface IEnemyView : IEnemyViewBase
    {
        IEnemyAnimation Animation { get; }
        FSMOwner FsmOwner { get; }
        EnemyDependencyProvider Provider { get; }
        ICharacterSpawnPoint CharacterMeleePoint { get; }
        ICharacterSpawnPoint CharacterRangePoint { get; }
        float FindRange { get; }


        void SetTargetFollow(ICharacterHealthView characterViewHealthView);
        void SetCharacterMeleePoint(ICharacterSpawnPoint spawnPoint);
        void SetCharacterRangePoint(ICharacterSpawnPoint spawnPoint);
    }
}