using Sources.BoundedContexts.CharacterRanges.Domain;
using Sources.BoundedContexts.CharacterRanges.Presentation.Implementation;
using Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces;

namespace Sources.BoundedContexts.CharacterRanges.Infrastructure.Factories.Views.Interfaces
{
    public interface ICharacterRangeViewFactory
    {
        ICharacterRangeView Create(CharacterRange characterRange);
        ICharacterRangeView Create(CharacterRange characterRange, CharacterRangeView view);
    }
}