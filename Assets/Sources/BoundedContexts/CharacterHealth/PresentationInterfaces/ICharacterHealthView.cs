using UnityEngine;

namespace Sources.BoundedContexts.CharacterHealth.PresentationInterfaces
{
    public interface ICharacterHealthView
    {
        Vector3 Position { get; }
        
        void TakeDamage(int damage);
        void PlayHealParticle();
    }
}