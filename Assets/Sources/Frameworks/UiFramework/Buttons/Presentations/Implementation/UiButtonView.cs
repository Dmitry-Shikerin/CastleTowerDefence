using Sirenix.OdinInspector;
using Sources.Presentations.Views;
using Sources.PresentationsInterfaces.UI.Buttons;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Sources.Frameworks.UiFramework.Presentation.Buttons
{
    public class UiButtonView : View, IButtonView
    {
        [TabGroup("Components")]
        [SerializeField] private Button _button;
        
        public void AddClickListener(UnityAction onClick) =>
            _button.onClick.AddListener(onClick);

        public void RemoveClickListener(UnityAction onClick) =>
            _button.onClick.RemoveListener(onClick);

        public void Enable() =>
            _button.enabled = true;

        public void Disable() =>
            _button.enabled = false;

    }
}