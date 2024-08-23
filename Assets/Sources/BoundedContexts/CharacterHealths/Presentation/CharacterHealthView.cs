using Sirenix.OdinInspector;
using Sources.BoundedContexts.CharacterHealths.Controllers;
using Sources.BoundedContexts.CharacterHealths.PresentationInterfaces;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using UnityEngine;

namespace Sources.BoundedContexts.CharacterHealths.Presentation
{
    public class CharacterHealthView : PresentableView<CharacterHealthPresenter>, ICharacterHealthView
    {
        [Required] [SerializeField] private ParticleSystem _healParticleSystem;

        public Vector3 Position => transform.position;
        public float CurrentHealth => Presenter.CurrentHealth;

        public void TakeDamage(int damage) =>
            Presenter.TakeDamage(damage);

        public void PlayHealParticle() =>
            _healParticleSystem.Play();
    }
}