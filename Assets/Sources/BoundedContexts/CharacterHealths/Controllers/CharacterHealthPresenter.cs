using System;
using Sources.BoundedContexts.CharacterHealths.Domain;
using Sources.BoundedContexts.CharacterHealths.PresentationInterfaces;
using Sources.Frameworks.MVPPassiveView.Controllers.Implementation;

namespace Sources.BoundedContexts.CharacterHealths.Controllers
{
    public class CharacterHealthPresenter : PresenterBase
    {
        private readonly ICharacterHealthView _characterHealthView;
        private readonly CharacterHealth _characterHealth;

        public CharacterHealthPresenter(
            CharacterHealth characterHealth,
            ICharacterHealthView characterHealthView)
        {
            _characterHealth = characterHealth ?? throw new ArgumentNullException(nameof(characterHealth));
            _characterHealthView = characterHealthView ?? throw new ArgumentNullException(nameof(characterHealthView));
        }

        public float CurrentHealth => _characterHealth.CurrentHealth;

        public void TakeDamage(int damage) =>
            _characterHealth.TakeDamage(damage);
    }
}