using Doozy.Runtime.UIManager.Components;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.EnemyHealths.Presentation.Interfaces;
using Sources.BoundedContexts.NukeAbilities.Controllers;
using Sources.BoundedContexts.NukeAbilities.Presentation.Interfaces;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using UnityEngine;

namespace Sources.BoundedContexts.NukeAbilities.Presentation.Implementation
{
    public class NukeAbilityView : PresentableView<NukeAbilityPresenter>, INukeAbilityView
    {
        [Required] [SerializeField] private ParticleSystem _nukeAbilityEffect;
        [Required] [SerializeField] private BombView _bombView;
        [Required] [SerializeField] private BoxCollider _damageCollider;
        // [Required] [SerializeField] private EnemyHealthTrigger _enemyHealthTrigger;

        public Vector3 DamageSize => _damageCollider.size;
        public IBombView BombView => _bombView;

        protected override void OnAfterEnable()
        {
            // _enemyHealthTrigger.Entered += OnEntered;
        }

        protected override void OnAfterDisable()
        {
            // _enemyHealthTrigger.Entered -= OnEntered;
        }
        
        public void PlayNukeParticle()
        {
            _nukeAbilityEffect.Play();
        }

        private void OnEntered(IEnemyHealthView enemyHealthView)
        {
            Presenter.DealDamage(enemyHealthView);
        }
    }
}