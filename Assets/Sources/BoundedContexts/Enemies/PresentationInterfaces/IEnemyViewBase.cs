using System.Collections.Generic;
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
        ITargetPoint TargetPoint { get; }
        
        void SetTargetPoint(ITargetPoint targetPointView);
        void SetCharacterHealth(ICharacterHealthView characterHealthView);
        void EnableNavmeshAgent();
        void DisableNavmeshAgent();
    }
}