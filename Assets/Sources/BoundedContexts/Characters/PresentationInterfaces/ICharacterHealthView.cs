namespace Sources.BoundedContexts.Characters.Controllers
{
    public interface ICharacterHealthView
    {
        void TakeDamage(int damage);
        void PlayHealParticle();
    }
}