using Sirenix.OdinInspector;
using Sources.BoundedContexts.Enemies.Controllers;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.Presentations.Views;
using UnityEngine;

namespace Sources.BoundedContexts.Enemies.Presentation
{
    public class EnemyHealthView : PresentableView<EnemyHealthPresenter>, IEnemyHealthView
    {
        [Required] [SerializeField] private ParticleSystem _bloodParticle;
        
        public Vector3 Position => transform.position;
        public float CurrentHealth => Presenter.CurrentHealth;

        public void TakeDamage(float damage) =>
            Presenter.TakeDamage(damage);

        public void PlayBloodParticle() =>
            _bloodParticle.Play();
    }
}