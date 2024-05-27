namespace Sources.BoundedContexts.EnemyAttackers.Domain
{
    public class EnemyAttacker
    {
        public EnemyAttacker(int attack)
        {
            Attack = attack;
        }

        public int Attack { get; }
    }
}