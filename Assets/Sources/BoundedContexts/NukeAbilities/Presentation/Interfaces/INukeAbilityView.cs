using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.Views;
using UnityEngine;

namespace Sources.BoundedContexts.NukeAbilities.Presentation.Interfaces
{
    public interface INukeAbilityView : IView
    {
        Vector3 DamageSize { get; }
        IBombView BombView { get; }
        
        void PlayNukeParticle();
    }
}