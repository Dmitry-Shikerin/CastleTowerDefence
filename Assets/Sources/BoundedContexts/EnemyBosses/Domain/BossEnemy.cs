using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.Enemies.Domain.Models;
using Sources.BoundedContexts.EnemyAttackers.Domain;
using Sources.BoundedContexts.EnemyHealths.Domain;

namespace Sources.BoundedContexts.EnemyBosses.Domain
{
    public class BossEnemy : Enemy
    {
        public BossEnemy(
            EnemyHealth enemyHealth,
            EnemyAttacker enemyAttacker,
            float stunTime,
            float walkSpeed,
            float runSpeed) 
            : base(
                enemyHealth, 
                enemyAttacker)
        {
            StunTime = stunTime;
            WalkSpeed = walkSpeed;
            RunSpeed = runSpeed;
        }
        
        public float StunTime { get; }
        public float WalkSpeed { get; }
        public float RunSpeed { get; }
        public bool IsRun { get; set; }
    }
}