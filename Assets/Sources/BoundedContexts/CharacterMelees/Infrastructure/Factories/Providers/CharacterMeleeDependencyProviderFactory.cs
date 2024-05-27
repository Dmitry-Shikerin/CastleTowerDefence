using Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Providers;
using Sources.BoundedContexts.CharacterMelees.PresentationInterfaces;

namespace Sources.BoundedContexts.Characters.Infrastructure.Factories.Providers
{
    public class CharacterMeleeDependencyProviderFactory
    {
        public CharacterMeleeDependencyProvider Create(ICharacterMeleeView meleeView)
        {
            CharacterMeleeDependencyProvider provider = meleeView.Provider;
            provider.Construct(meleeView, meleeView.MeleeAnimation);

            return provider;
        }
    }
}