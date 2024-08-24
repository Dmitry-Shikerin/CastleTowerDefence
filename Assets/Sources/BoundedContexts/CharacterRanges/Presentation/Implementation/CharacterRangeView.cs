using Sirenix.OdinInspector;
using Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces;
using Sources.BoundedContexts.Characters.Presentation.Implementation;
using UnityEngine;

namespace Sources.BoundedContexts.CharacterRanges.Presentation.Implementation
{
    public class CharacterRangeView : CharacterView, ICharacterRangeView
    {
        [Required] [SerializeField] private ParticleSystem _shootParticle;
        
        public void PlayShootParticle() =>
            _shootParticle.Play();
    }
}