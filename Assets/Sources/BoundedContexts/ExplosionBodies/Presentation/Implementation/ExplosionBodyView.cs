using Sirenix.OdinInspector;
using Sources.BoundedContexts.ExplosionBodies.Presentation.Interfaces;
using Sources.Frameworks.GameServices.ObjectPools.Implementation.Destroyers;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Destroyers;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using UnityEngine;

namespace Sources.BoundedContexts.ExplosionBodies.Presentation.Implementation
{
    public class ExplosionBodyView : View, IExplosionBodyView
    {
        [Required] [SerializeField] private ParticleSystem _particleSystem;
        
        private readonly IPODestroyerService _poDestroyerService = 
            new PODestroyerService();
        
        private void OnEnable() =>
            _particleSystem.Play();

        private void OnParticleSystemStopped() =>
            _poDestroyerService.Destroy(this);
    }
}