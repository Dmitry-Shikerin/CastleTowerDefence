using Sources.BoundedContexts.CharacterMelees.Domain;
using Sources.BoundedContexts.CharacterMelees.Presentation;
using Sources.BoundedContexts.CharacterMelees.PresentationInterfaces;

namespace Sources.BoundedContexts.CharacterMelees.Infrastructure.Factories.Views.Interfaces
{
    public interface ICharacterMeleeViewFactory
    {
        ICharacterMeleeView Create(CharacterMelee characterMelee);
        ICharacterMeleeView Create(CharacterMelee characterMelee, CharacterMeleeView view);
    }
}