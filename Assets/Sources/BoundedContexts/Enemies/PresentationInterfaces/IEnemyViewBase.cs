using System.Collections.Generic;
using Sources.BoundedContexts.Bunkers.Presentation.Interfaces;
using Sources.BoundedContexts.CharacterHealth.PresentationInterfaces;
using Sources.BoundedContexts.NavMeshAgents.PresentationInterfaces;
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
        
        void SetBunkerView(IBunkerView bunkerView);
        void SetCharacterHealth(ICharacterHealthView characterHealthView);
        void EnableNavmeshAgent();
        void DisableNavmeshAgent();
    }
}