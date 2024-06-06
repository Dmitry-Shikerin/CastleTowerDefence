namespace Sources.BoundedContexts.EnemyAttackers.Domain
{
    public class EnemyAttacker
    {
        public EnemyAttacker(int damage)
        {
            Damage = damage;
        }

        public int Damage { get; }
    }
}