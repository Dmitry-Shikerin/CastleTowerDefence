using Sirenix.OdinInspector;
using Sources.BoundedContexts.FlamethrowerAbilities.Controllers;
using Sources.BoundedContexts.FlamethrowerAbilities.Presentation.Interfaces;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sources.BoundedContexts.FlamethrowerAbilities.Presentation.Implementation
{
    public class FlamethrowerAbilityView : PresentableView<FlamethrowerAbilityPresenter>, IFlamethrowerAbilityView
    {
        [Required] [SerializeField] private ParticleSystem _particleSystem;
        [Required] [SerializeField] private FlamethrowerView _flamethrowerView;

        public IFlamethrowerView FlamethrowerView => _flamethrowerView;

        public void PlayParticle()
        {
            _particleSystem.Play();
        }

        public void StopParticle()
        {
            _particleSystem.Stop();
        }
    }
}