using Doozy.Runtime.UIManager.Components;
using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.Views;
using UnityEngine;

namespace Sources.BoundedContexts.NukeAbilities.Presentation.Interfaces
{
    public interface INukeAbilityView : IView
    {
        Vector3 DamageSize { get; }
        UIButton NukeButton { get; }
        IBombView BombView { get; }

        void ShowNukePostprocess();
        void HideNukePostprocess();
        void ShowDarkPostProcess();
        void HideDarkPostProcess();
        void PlayNukeParticle();
    }
}