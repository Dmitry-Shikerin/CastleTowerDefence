using Sources.BoundedContexts.CharacterRanges.Domain;
using Sources.BoundedContexts.CharacterRanges.Infrastructure.Services.Providers;
using Sources.BoundedContexts.CharacterRanges.Presentation.Implementation;

namespace Sources.BoundedContexts.CharacterRanges.Infrastructure.Factories.Services
{
    public class CharacterRangeDependencyProviderFactory
    {
        public CharacterRangeDependencyProvider Create(
            CharacterRange characterRange, 
            CharacterRangeView view)
        {
            view.Provider.Construct(
                characterRange, 
                view,
                view.RangeAnimation);
            
            return view.Provider;
        }
    }
}