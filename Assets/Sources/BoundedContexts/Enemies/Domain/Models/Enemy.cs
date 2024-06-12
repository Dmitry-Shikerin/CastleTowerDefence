using System;
using Sources.BoundedContexts.BurnAbilities.Domain;
using Sources.BoundedContexts.EnemyAttackers.Domain;
using Sources.BoundedContexts.EnemyHealths.Domain;
using UnityEngine;

namespace Sources.BoundedContexts.Enemies.Domain.Models
{
    public class Enemy
    {
        public Enemy(
            EnemyHealth enemyHealth,
            EnemyAttacker enemyAttacker,
            BurnAbility burnAbility)
        {
            EnemyHealth = enemyHealth ?? throw new ArgumentNullException(nameof(enemyHealth));
            EnemyAttacker = enemyAttacker;
            BurnAbility = burnAbility;
        }

        public bool IsInitialized { get; set; }
        public EnemyHealth EnemyHealth { get; set; }
        public EnemyAttacker EnemyAttacker { get; }
        public BurnAbility BurnAbility { get; }
    }
}