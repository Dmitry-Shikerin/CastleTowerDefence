using UnityEngine;

namespace Sources.BoundedContexts.EnemyHealths.Presentation.Interfaces
{
    public interface IEnemyHealthView
    {
        Vector3 Position { get; }
        float CurrentHealth { get; }
        
        void TakeDamage(float damage);
        public void PlayBloodParticle();
    }
}