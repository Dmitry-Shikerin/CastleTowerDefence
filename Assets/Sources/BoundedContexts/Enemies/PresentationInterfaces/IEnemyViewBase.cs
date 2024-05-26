using System.Collections.Generic;
using Sources.BoundedContexts.CharacterHealth.PresentationInterfaces;
using Sources.BoundedContexts.Skins.PresentationInterfaces;
using Sources.PresentationsInterfaces.Views;

namespace Sources.BoundedContexts.Enemies.PresentationInterfaces
{
    public interface IEnemyViewBase : IView
    {
        IReadOnlyList<ISkinView> Skins { get; }
        ICharacterHealthView CharacterHealthView { get; }
        
        void SetCharacterHealth(ICharacterHealthView characterHealthView);
        void EnableNavmeshAgent();
        void DisableNavmeshAgent();
    }
}