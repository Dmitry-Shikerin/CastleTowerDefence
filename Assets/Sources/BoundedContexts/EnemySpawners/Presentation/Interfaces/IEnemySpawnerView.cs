using System.Collections.Generic;
using Sources.BoundedContexts.Bunkers.Presentation.Interfaces;
using Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces;
using Sources.BoundedContexts.SpawnPoints.Presentation.Implementation;
using Sources.BoundedContexts.TargetPoints.Presentation.Interfaces;

namespace Sources.BoundedContexts.EnemySpawners.Presentation.Interfaces
{
    public interface IEnemySpawnerView
    {
        IReadOnlyList<IEnemySpawnPoint> SpawnPoints { get; }
        IBunkerView BunkerView { get; }
        ICharacterMeleeView CharacterMeleeView { get; }

        void StartSpawn();
        void SetCharacterView(ICharacterMeleeView characterMeleeView);
        void SetBunkerView(IBunkerView bunkerView);
    }
}