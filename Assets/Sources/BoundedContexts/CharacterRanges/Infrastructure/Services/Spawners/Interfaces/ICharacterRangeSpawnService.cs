using Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces;
using Sources.BoundedContexts.Upgrades.Domain.Models;
using UnityEngine;

namespace Sources.BoundedContexts.CharacterRanges.Infrastructure.Services.Spawners.Interfaces
{
    public interface ICharacterRangeSpawnService
    {
        public ICharacterRangeView Spawn(Vector3 position, Upgrade characterHealthUpgrade);
    }
}