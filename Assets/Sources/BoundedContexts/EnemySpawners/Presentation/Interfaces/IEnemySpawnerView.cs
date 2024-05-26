using System.Collections.Generic;
using Sources.BoundedContexts.Characters.Presentation;
using Sources.BoundedContexts.Characters.PresentationInterfaces;
using Sources.BoundedContexts.SpawnPoints.Presentation;

namespace Sources.BoundedContexts.EnemySpawners.Presentationinterfaces
{
    public interface IEnemySpawnerView
    {
        IReadOnlyList<SpawnPoint> SpawnPoints { get; }
        ICharacterView CharacterView { get; }
        
        void SetCharacterView(ICharacterView characterView);
    }
}