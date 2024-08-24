using Sources.BoundedContexts.CharacterHealths.Domain;

namespace Sources.BoundedContexts.Characters.Domain
{
    public class Character
    {
        public Character(CharacterHealth characterHealth)
        {
            CharacterHealth = characterHealth;
        }
        
        public CharacterHealth CharacterHealth { get; }
        public bool IsInitialized { get; set; }
    }
}