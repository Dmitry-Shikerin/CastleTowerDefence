using Sirenix.OdinInspector;
using Sources.BoundedContexts.BurnAbilities.Presentation.Interfaces;
using Sources.BoundedContexts.FlamethrowerAbilities.Controllers;
using Sources.BoundedContexts.FlamethrowerAbilities.Presentation.Interfaces;
using Sources.BoundedContexts.Triggers.Presentation;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sources.BoundedContexts.FlamethrowerAbilities.Presentation.Implementation
{
    public class FlamethrowerAbilityView : PresentableView<FlamethrowerAbilityPresenter>, IFlamethrowerAbilityView
    {
        [Required] [SerializeField] private ParticleSystem _particleSystem;
        [Required] [SerializeField] private FlamethrowerView _flamethrowerView;
        [FormerlySerializedAs("burnAbilityCollicion")]
        [FormerlySerializedAs("_burnAbilityTrigger")]
        [Space(5)] 
        [Required] [SerializeField] private BurnAbilityCollision burnAbilityCollision;

        public IFlamethrowerView FlamethrowerView => _flamethrowerView;

        protected override void OnAfterEnable() =>
            burnAbilityCollision.Entered += OnEnter;

        protected override void OnAfterDisable() =>
            burnAbilityCollision.Entered -= OnEnter;

        private void OnEnter(IBurnable obj)
        {
            Presenter.DealDamage(obj);
        }

        public void PlayParticle() =>
            _particleSystem.Play();

        public void StopParticle() =>
            _particleSystem.Stop();
    }
}