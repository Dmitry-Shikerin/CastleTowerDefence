namespace Sources.BoundedContexts.CharacterMelees.Domain
{
    public class CharacterMelee
    {
        public Characters.CharacterHealth CharacterHealth { get; }

        public CharacterMelee(Characters.CharacterHealth characterHealth)
        {
            CharacterHealth = characterHealth;
        }
    }
}