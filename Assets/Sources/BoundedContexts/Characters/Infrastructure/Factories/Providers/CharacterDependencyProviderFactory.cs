using Sources.BoundedContexts.Characters.Infrastructure.Services.Providers;
using Sources.BoundedContexts.Characters.PresentationInterfaces;

namespace Sources.BoundedContexts.Characters.Infrastructure.Factories.Providers
{
    public class CharacterDependencyProviderFactory
    {
        public CharacterDependencyProvider Create(ICharacterView view)
        {
            CharacterDependencyProvider provider = view.Provider;
            provider.Construct(view, view.Animation);

            return provider;
        }
    }
}