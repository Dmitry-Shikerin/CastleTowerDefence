using Sources.BoundedContexts.CharacterHealths.PresentationInterfaces;
using Sources.BoundedContexts.SpawnPoints.Presentation.Interfaces;

namespace Sources.BoundedContexts.CharacterSpawners.Presentation.Interfaces
{
    public interface ICharacterSpawnPoint : ISpawnPoint
    {
        ICharacterHealthView CharacterHealthView { get; }
        
        void SetCharacterHealth(ICharacterHealthView characterHealthView);
    }
}