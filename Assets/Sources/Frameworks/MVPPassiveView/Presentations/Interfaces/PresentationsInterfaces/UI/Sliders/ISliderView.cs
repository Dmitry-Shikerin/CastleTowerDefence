using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.Views;
using UnityEngine.Events;

namespace Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.UI.Sliders
{
    public interface ISliderView : IView
    {
        void SetValue(float value);
        void AddListener(UnityAction<float> action);
        void RemoveListener(UnityAction<float> action);
    }
}