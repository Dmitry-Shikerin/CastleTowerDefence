namespace Sources.BoundedContexts.CharacterHealth.PresentationInterfaces
{
    public interface ICharacterHealthView
    {
        void TakeDamage(int damage);
        void PlayHealParticle();
    }
}