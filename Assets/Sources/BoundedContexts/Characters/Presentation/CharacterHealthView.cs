using Sirenix.OdinInspector;
using Sources.BoundedContexts.Characters.Controllers;
using Sources.Presentations.Views;
using UnityEngine;

namespace Sources.BoundedContexts.Characters.Presentation
{
    public class CharacterHealthView : PresentableView<CharacterHealthPresenter>, ICharacterHealthView
    {
        [Required] [SerializeField] private ParticleSystem _healParticleSystem;
        
        public void TakeDamage(int damage) =>
            Presenter.TakeDamage(damage);

        public void PlayHealParticle() =>
            _healParticleSystem.Play();
    }
}