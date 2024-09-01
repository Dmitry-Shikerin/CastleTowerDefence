using Sources.BoundedContexts.Bunkers.Presentation.Interfaces;
using Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces;

namespace Sources.BoundedContexts.EnemySpawners.Presentation.Interfaces
{
    public interface IEnemySpawnerView
    {
        IBunkerView BunkerView { get; }
        ICharacterMeleeView CharacterMeleeView { get; }
        IEnemySpawnPoint[,] SpawnPoints { get; }

        void StartSpawn();
        void SetCharacterView(ICharacterMeleeView characterMeleeView);
        void SetBunkerView(IBunkerView bunkerView);
    }
}