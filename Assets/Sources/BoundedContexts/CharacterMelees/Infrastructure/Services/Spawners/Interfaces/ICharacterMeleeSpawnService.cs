using Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces;
using Sources.BoundedContexts.Upgrades.Domain.Models;
using UnityEngine;

namespace Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Spawners.Interfaces
{
    public interface ICharacterMeleeSpawnService
    {
        public ICharacterMeleeView Spawn(Vector3 position, Upgrade characterHealthUpgrade);
    }
}