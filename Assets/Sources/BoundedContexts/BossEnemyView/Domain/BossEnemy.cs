using Sources.BoundedContexts.Enemies.Domain;

namespace Sources.BoundedContexts.BossEnemyView.Domain
{
    public class BossEnemy
    {
        public BossEnemy(EnemyHealth enemyHealth)
        {
            EnemyHealth = enemyHealth;
        }

        public EnemyHealth EnemyHealth { get; }
    }
}