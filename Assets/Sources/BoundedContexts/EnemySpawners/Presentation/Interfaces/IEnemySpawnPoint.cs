using Sources.BoundedContexts.CharacterSpawnAbilities.Presentation.Interfaces;
using Sources.BoundedContexts.SpawnPoints.Presentation.Interfaces;

namespace Sources.BoundedContexts.EnemySpawners.Presentation.Interfaces
{
    public interface IEnemySpawnPoint : ISpawnPoint
    {
        ICharacterSpawnPoint CharacterMeleeSpawnPoint { get; }
        ICharacterSpawnPoint CharacterRangedSpawnPoint { get; }
    }
}