using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.Views;
using UnityEngine.Events;

namespace Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.UI.Buttons
{
    public interface IButtonView : IView
    {
        void AddClickListener(UnityAction onClick);
        void RemoveClickListener(UnityAction onClick);
        void Enable();
        void Disable();
    }
}