using Sources.BoundedContexts.Characters.Presentation.Interfaces;

namespace Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces
{
    public interface ICharacterRangeView : ICharacterView
    {
        void PlayShootParticle();
    }
}