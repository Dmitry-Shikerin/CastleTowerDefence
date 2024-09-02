using System.Collections.Generic;
using Sources.BoundedContexts.Bunkers.Presentation.Interfaces;
using Sources.BoundedContexts.CharacterHealths.PresentationInterfaces;
using Sources.BoundedContexts.CharacterSpawnAbilities.Presentation.Interfaces;
using Sources.BoundedContexts.EnemyHealths.Presentation.Implementation;
using Sources.BoundedContexts.NavMeshAgents.Presentation.Interfaces;
using Sources.BoundedContexts.Skins.PresentationInterfaces;

namespace Sources.BoundedContexts.Enemies.PresentationInterfaces
{
    public interface IEnemyViewBase : INavMeshAgent
    {
        IReadOnlyList<ISkinView> Skins { get; }
        ICharacterHealthView CharacterHealthView { get; }
        IBunkerView BunkerView { get; }
        EnemyHealthView EnemyHealthView { get; }
        ICharacterSpawnPoint CharacterMeleePoint { get; }
        
        void SetBunkerView(IBunkerView bunkerView);
        public void SetCharacterMeleePoint(ICharacterSpawnPoint characterSpawnPoint);
        void SetCharacterHealth(ICharacterHealthView characterHealthView);
    }
}