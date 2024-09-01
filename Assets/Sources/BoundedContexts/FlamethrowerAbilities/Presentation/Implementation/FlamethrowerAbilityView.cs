using Sirenix.OdinInspector;
using Sources.BoundedContexts.BurnAbilities.Presentation.Interfaces;
using Sources.BoundedContexts.FlamethrowerAbilities.Controllers;
using Sources.BoundedContexts.FlamethrowerAbilities.Presentation.Interfaces;
using Sources.BoundedContexts.Triggers.Presentation;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using UnityEngine;

namespace Sources.BoundedContexts.FlamethrowerAbilities.Presentation.Implementation
{
    public class FlamethrowerAbilityView : PresentableView<FlamethrowerAbilityPresenter>, IFlamethrowerAbilityView
    {
        [Required] [SerializeField] private ParticleSystem _particleSystem;
        [Required] [SerializeField] private FlamethrowerView _flamethrowerView;
        [Required] [SerializeField] private BurnAbilityCollision _burnAbilityCollision;
        
        public IFlamethrowerView FlamethrowerView => _flamethrowerView;

        protected override void OnAfterEnable() =>
            _burnAbilityCollision.Entered += OnEnter;

        protected override void OnAfterDisable() =>
            _burnAbilityCollision.Entered -= OnEnter;

        private void OnEnter(IBurnable obj) =>
            Presenter.DealDamage(obj);

        public void PlayParticle() =>
            _particleSystem.Play();

        public void StopParticle() =>
            _particleSystem.Stop();
    }
}