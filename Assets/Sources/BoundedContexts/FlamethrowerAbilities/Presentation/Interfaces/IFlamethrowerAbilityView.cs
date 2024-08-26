using UnityEngine;

namespace Sources.BoundedContexts.FlamethrowerAbilities.Presentation.Interfaces
{
    public interface IFlamethrowerAbilityView
    {
        Transform Transform { get; }
        
        public IFlamethrowerView FlamethrowerView { get; }
        void PlayParticle();
        void StopParticle();
    }
}