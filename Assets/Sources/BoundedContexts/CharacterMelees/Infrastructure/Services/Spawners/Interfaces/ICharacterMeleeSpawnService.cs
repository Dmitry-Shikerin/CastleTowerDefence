using Sources.BoundedContexts.CharacterMelees.PresentationInterfaces;
using UnityEngine;

namespace Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Spawners.Interfaces
{
    public interface ICharacterMeleeSpawnService
    {
        public ICharacterMeleeView Spawn(Vector3 position);
    }
}