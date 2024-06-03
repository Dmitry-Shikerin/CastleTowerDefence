using Sources.BoundedContexts.CharacterHealth.PresentationInterfaces;
using Sources.BoundedContexts.CharacterSpawners.Presentation.Interfaces;
using Sources.BoundedContexts.SpawnPoints.Presentation.Implementation;

namespace Sources.BoundedContexts.CharacterSpawners.Presentation.Implementation
{
    public class CharacterSpawnPoint : SpawnPoint, ICharacterSpawnPoint
    {
        public ICharacterHealthView CharacterHealthView { get; private set; }
        
        public void SetCharacterHealth(ICharacterHealthView characterHealthView) =>
            CharacterHealthView = characterHealthView;
    }
}