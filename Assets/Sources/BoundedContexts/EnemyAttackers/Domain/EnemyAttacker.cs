namespace Sources.BoundedContexts.EnemyAttackers.Domain
{
    public class EnemyAttacker
    {
        public EnemyAttacker(
            int damage,
            int massAttackDamage)
        {
            Damage = damage;
            MassAttackDamage = massAttackDamage;
        }

        public int Damage { get; }
        public int MassAttackDamage { get; }
    }
}