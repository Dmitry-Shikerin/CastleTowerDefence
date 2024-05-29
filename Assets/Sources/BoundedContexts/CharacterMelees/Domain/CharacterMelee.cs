namespace Sources.BoundedContexts.CharacterMelees.Domain
{
    public class CharacterMelee
    {
        public CharacterHealth.Domain.CharacterHealth CharacterHealth { get; }

        public CharacterMelee(CharacterHealth.Domain.CharacterHealth characterHealth)
        {
            CharacterHealth = characterHealth;
        }
    }
}