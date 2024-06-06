using Sources.BoundedContexts.CharacterHealths.Domain;

namespace Sources.BoundedContexts.CharacterRanges.Domain
{
    public class CharacterRange
    {

        public CharacterRange(CharacterHealths.Domain.CharacterHealth characterHealth)
        {
            CharacterHealth = characterHealth;
        }
        
        public CharacterHealths.Domain.CharacterHealth CharacterHealth { get; }
        public bool IsInitialized { get; set; }
    }
}