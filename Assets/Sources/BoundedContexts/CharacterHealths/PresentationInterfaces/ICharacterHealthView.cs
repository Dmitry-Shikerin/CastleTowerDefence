using UnityEngine;

namespace Sources.BoundedContexts.CharacterHealths.PresentationInterfaces
{
    public interface ICharacterHealthView
    {
        Vector3 Position { get; }
        float CurrentHealth { get; }
        
        void TakeDamage(int damage);
        void PlayHealParticle();
    }
}