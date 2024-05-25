using UnityEngine;

namespace Sources.BoundedContexts.Enemies.PresentationInterfaces
{
    public interface IEnemyHealthView
    {
        Vector3 Position { get; }
        float CurrentHealth { get; }
        
        void TakeDamage(float damage);
        public void PlayBloodParticle();
    }
}