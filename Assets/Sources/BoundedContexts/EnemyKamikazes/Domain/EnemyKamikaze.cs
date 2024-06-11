using System;
using Sources.BoundedContexts.BurnAbilities.Domain;
using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.Enemies.Domain.Models;
using Sources.BoundedContexts.EnemyAttackers.Domain;
using Sources.BoundedContexts.EnemyHealths.Domain;

namespace Sources.BoundedContexts.EnemyKamikazes.Domain
{
    public class EnemyKamikaze : Enemy
    {
        public EnemyKamikaze(
            EnemyHealth enemyHealth,
            EnemyAttacker enemyAttacker,
            BurnAbility burnAbility) 
            : base(
                enemyHealth, 
                enemyAttacker, 
                burnAbility)
        {
        }
    }
}