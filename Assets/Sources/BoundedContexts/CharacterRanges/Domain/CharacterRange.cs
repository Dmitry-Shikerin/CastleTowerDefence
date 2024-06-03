namespace Sources.BoundedContexts.CharacterRanges.Domain
{
    public class CharacterRange
    {
        public CharacterHealths.Domain.CharacterHealth CharacterHealth { get; }

        public CharacterRange(CharacterHealths.Domain.CharacterHealth characterHealth)
        {
            CharacterHealth = characterHealth;
        }
    }
}