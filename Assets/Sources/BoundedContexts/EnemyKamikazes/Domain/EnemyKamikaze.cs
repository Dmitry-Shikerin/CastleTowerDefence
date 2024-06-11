using System;
using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.EnemyAttackers.Domain;
using Sources.BoundedContexts.EnemyHealths.Domain;

namespace Sources.BoundedContexts.EnemyKamikazes.Domain
{
    public class EnemyKamikaze
    {
        public EnemyKamikaze(
            EnemyHealth enemyHealth,
            EnemyAttacker enemyAttacker)
        {
            EnemyHealth = enemyHealth ?? throw new ArgumentNullException(nameof(enemyHealth));
            EnemyAttacker = enemyAttacker;
        }

        public bool IsInitialized { get; set; }
        public EnemyHealth EnemyHealth { get; set; }
        public EnemyAttacker EnemyAttacker { get; }
    }
}