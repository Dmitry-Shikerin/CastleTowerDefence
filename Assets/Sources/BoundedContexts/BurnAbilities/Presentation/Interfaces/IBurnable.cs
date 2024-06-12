namespace Sources.BoundedContexts.BurnAbilities.Presentation.Interfaces
{
    public interface IBurnable
    {
        void Burn(int instantDamage, int overtimeDamage);
    }
}