namespace Sources.BoundedContexts.CharacterRanges.Domain
{
    public class CharacterRange
    {
        public CharacterHealth.Domain.CharacterHealth CharacterHealth { get; }

        public CharacterRange(CharacterHealth.Domain.CharacterHealth characterHealth)
        {
            CharacterHealth = characterHealth;
        }
    }
}