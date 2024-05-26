using System;
using Sources.BoundedContexts.CharacterHealth.PresentationInterfaces;
using Sources.BoundedContexts.Characters.Presentation;
using Sources.Controllers.Common;

namespace Sources.BoundedContexts.Characters.Controllers
{
    public class CharacterHealthPresenter : PresenterBase
    {
        private readonly ICharacterHealthView _characterHealthView;
        private readonly CharacterHealth _characterHealth;

        public CharacterHealthPresenter(CharacterHealth characterHealth,
            ICharacterHealthView characterHealthView)
        {
            _characterHealth = characterHealth ?? throw new ArgumentNullException(nameof(characterHealth));
            _characterHealthView = characterHealthView ?? throw new ArgumentNullException(nameof(characterHealthView));
        }

        public void TakeDamage(int damage) =>
            _characterHealth.TakeDamage(damage);
    }
}