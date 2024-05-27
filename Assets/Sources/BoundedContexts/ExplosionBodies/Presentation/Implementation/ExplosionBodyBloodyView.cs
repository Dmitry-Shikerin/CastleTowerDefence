using Sirenix.OdinInspector;
using Sources.BoundedContexts.ExplosionBodies.Presentation.Interfaces;
using Sources.Frameworks.Services.ObjectPools.Implementation.Destroyers;
using Sources.Frameworks.Services.ObjectPools.Interfaces.Destroyers;
using Sources.Presentations.Views;
using UnityEngine;

namespace Sources.BoundedContexts.ExplosionBodies.Presentation.Implementation
{
    public class ExplosionBodyBloodyView : View, IExplosionBodyBloodyView
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