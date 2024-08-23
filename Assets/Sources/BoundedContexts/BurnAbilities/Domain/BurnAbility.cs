namespace Sources.BoundedContexts.BurnAbilities.Domain
{
    public class BurnAbility
    {
        public float Cooldown { get; } = 1.5f;
        public bool IsAvailable { get; set; } = true;
        public double BurnTick { get; set; } = 4f;
    }
}