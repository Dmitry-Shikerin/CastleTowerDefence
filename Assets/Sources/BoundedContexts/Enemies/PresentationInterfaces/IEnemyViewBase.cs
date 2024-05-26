using System.Collections.Generic;
using Sources.BoundedContexts.CharacterHealth.PresentationInterfaces;
using Sources.BoundedContexts.Characters.Controllers;
using Sources.BoundedContexts.Skins.PresentationInterfaces;

namespace Sources.BoundedContexts.Enemies.PresentationInterfaces
{
    public interface IEnemyViewBase
    {
        IReadOnlyList<ISkinView> Skins { get; }
        ICharacterHealthView CharacterHealthView { get; }
        
        void SetCharacterHealth(ICharacterHealthView characterHealthView);
        void EnableNavmeshAgent();
        void DisableNavmeshAgent();
    }
}