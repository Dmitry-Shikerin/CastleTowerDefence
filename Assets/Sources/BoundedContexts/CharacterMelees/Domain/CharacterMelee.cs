namespace Sources.BoundedContexts.CharacterMelees.Domain
{
    public class CharacterMelee
    {

        public CharacterMelee(CharacterHealth.Domain.CharacterHealth characterHealth)
        {
            CharacterHealth = characterHealth;
        }
        
        public CharacterHealth.Domain.CharacterHealth CharacterHealth { get; }
        public bool IsInitialized { get; set; }
    }
}