namespace Sources.BoundedContexts.CharacterMelees.Domain
{
    public class CharacterMelee
    {

        public CharacterMelee(CharacterHealths.Domain.CharacterHealth characterHealth)
        {
            CharacterHealth = characterHealth;
        }
        
        public CharacterHealths.Domain.CharacterHealth CharacterHealth { get; }
        public bool IsInitialized { get; set; }
    }
}