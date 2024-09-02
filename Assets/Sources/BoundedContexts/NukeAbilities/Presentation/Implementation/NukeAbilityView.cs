using Sirenix.OdinInspector;
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

        public Vector3 DamageSize => _damageCollider.size;
        public IBombView BombView => _bombView;
        
        public void PlayNukeParticle() =>
            _nukeAbilityEffect.Play();
    }
}