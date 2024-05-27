using System.Collections.Generic;
using Sources.BoundedContexts.CharacterMelees.PresentationInterfaces;
using Sources.BoundedContexts.SpawnPoints.Presentation.Implementation;
using Sources.BoundedContexts.TargetPoints.Presentation.Interfaces;

namespace Sources.BoundedContexts.EnemySpawners.Presentation.Interfaces
{
    public interface IEnemySpawnerView
    {
        IReadOnlyList<SpawnPoint> SpawnPoints { get; }
        ITargetPoint TargetPoint { get; }
        ICharacterMeleeView CharacterMeleeView { get; }
        
        void SetCharacterView(ICharacterMeleeView characterMeleeView);
    }
}