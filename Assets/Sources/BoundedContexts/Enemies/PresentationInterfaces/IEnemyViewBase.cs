using System.Collections.Generic;
using Sources.BoundedContexts.Bunkers.Presentation.Interfaces;
using Sources.BoundedContexts.CharacterHealth.PresentationInterfaces;
using Sources.BoundedContexts.EnemyHealths.Presentation.Implementation;
using Sources.BoundedContexts.NavMeshAgents.Presentation.Interfaces;
using Sources.BoundedContexts.Skins.PresentationInterfaces;
using Sources.BoundedContexts.TargetPoints.Presentation.Interfaces;
using Sources.PresentationsInterfaces.Views;

namespace Sources.BoundedContexts.Enemies.PresentationInterfaces
{
    public interface IEnemyViewBase : INavMeshAgent
    {
        IReadOnlyList<ISkinView> Skins { get; }
        ICharacterHealthView CharacterHealthView { get; }
        IBunkerView BunkerView { get; }
        EnemyHealthView EnemyHealthView { get; }
        
        void SetBunkerView(IBunkerView bunkerView);
        void SetCharacterHealth(ICharacterHealthView characterHealthView);
        void EnableNavmeshAgent();
        void DisableNavmeshAgent();
    }
}