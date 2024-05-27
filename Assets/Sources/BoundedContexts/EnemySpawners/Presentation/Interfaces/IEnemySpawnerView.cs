using System.Collections.Generic;
using Sources.BoundedContexts.CharacterMelees.PresentationInterfaces;
using Sources.BoundedContexts.SpawnPoints.Presentation;
using Sources.BoundedContexts.SpawnPoints.Presentation.Implementation;

namespace Sources.BoundedContexts.EnemySpawners.Presentationinterfaces
{
    public interface IEnemySpawnerView
    {
        IReadOnlyList<SpawnPoint> SpawnPoints { get; }
        ICharacterMeleeView CharacterMeleeView { get; }
        
        void SetCharacterView(ICharacterMeleeView characterMeleeView);
    }
}