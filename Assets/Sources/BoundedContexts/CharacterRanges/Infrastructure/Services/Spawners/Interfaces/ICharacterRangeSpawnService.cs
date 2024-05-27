using Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces;
using UnityEngine;

namespace Sources.BoundedContexts.CharacterRanges.Infrastructure.Services.Spawners.Interfaces
{
    public interface ICharacterRangeSpawnService
    {
        public ICharacterRangeView Spawn(Vector3 position);
    }
}