using Sources.BoundedContexts.CharacterHealths.Domain;

namespace Sources.BoundedContexts.CharacterRanges.Domain
{
    public class CharacterRange
    {

        public CharacterRange(CharacterHealth characterHealth)
        {
            CharacterHealth = characterHealth;
        }
        
        public CharacterHealth CharacterHealth { get; }
        public bool IsInitialized { get; set; }
    }
}