namespace Sources.BoundedContexts.CharacterRanges.Domain
{
    public class CharacterRange
    {
        public Characters.CharacterHealth CharacterHealth { get; }

        public CharacterRange(Characters.CharacterHealth characterHealth)
        {
            CharacterHealth = characterHealth;
        }
    }
}