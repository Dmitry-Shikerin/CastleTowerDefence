using System;
using Sources.BoundedContexts.Characters.Infrastructure.Factories.Providers;
using Sources.BoundedContexts.Characters.Infrastructure.Services.Providers;
using Sources.BoundedContexts.Characters.Presentation;
using Sources.BoundedContexts.Characters.PresentationInterfaces;

namespace Sources.BoundedContexts.Characters.Infrastructure.Factories.Views
{
    public class CharacterViewFactory
    {
        private readonly CharacterDependencyProviderFactory _providerFactory;

        public CharacterViewFactory(CharacterDependencyProviderFactory providerFactory)
        {
            _providerFactory = providerFactory ?? throw new ArgumentNullException(nameof(providerFactory));
        }
        
        public ICharacterView Create(CharacterView view)
        {
            CharacterDependencyProvider provider = _providerFactory.Create(view);
            
            return view;
        }
    }
}