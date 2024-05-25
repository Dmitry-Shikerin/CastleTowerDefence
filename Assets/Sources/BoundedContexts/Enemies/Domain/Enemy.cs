using System;
using Sources.BoundedContexts.EnemyAttackers.Domain;

namespace Sources.BoundedContexts.Enemies.Domain
{
    public class Enemy
    {
        public Enemy(
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