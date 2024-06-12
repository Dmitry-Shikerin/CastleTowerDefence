using Sirenix.OdinInspector;
using Sources.BoundedContexts.BurnAbilities.Controllers;
using Sources.BoundedContexts.BurnAbilities.Presentation.Interfaces;
using Sources.BoundedContexts.EnemyHealths.Presentation.Implementation;
using Sources.BoundedContexts.EnemyHealths.Presentation.Interfaces;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using UnityEngine;

namespace Sources.BoundedContexts.BurnAbilities.Presentation.Implementation
{
    public class BurnAbilityView : PresentableView<BurnAbilityPresenter>, IBurnAbilityView
    {
        [Required] [SerializeField] private ParticleSystem _particleSystem;
        [Required] [SerializeField] private EnemyHealthView _enemyHealthView;

        public IEnemyHealthView EnemyHealthView => _enemyHealthView;

        public void PlayBurnParticle() =>
            _particleSystem.Play();

        public void StopBurnParticle() =>
            _particleSystem.Stop();

        public void Burn(int instantDamage, int overtimeDamage) =>
            Presenter.Burn(instantDamage, overtimeDamage);
    }
}