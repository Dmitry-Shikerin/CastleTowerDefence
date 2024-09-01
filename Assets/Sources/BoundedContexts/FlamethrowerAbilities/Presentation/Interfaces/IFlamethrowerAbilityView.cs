using UnityEngine;

namespace Sources.BoundedContexts.FlamethrowerAbilities.Presentation.Interfaces
{
    public interface IFlamethrowerAbilityView
    {
        public IFlamethrowerView FlamethrowerView { get; }
        void PlayParticle();
        void StopParticle();
    }
}