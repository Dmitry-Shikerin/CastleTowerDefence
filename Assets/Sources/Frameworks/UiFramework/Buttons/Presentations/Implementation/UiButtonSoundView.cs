using Sirenix.OdinInspector;
using Sources.PresentationsInterfaces.UI.Buttons;
using UnityEngine;

namespace Sources.Frameworks.UiFramework.Presentation.Buttons
{
    public class UiButtonSoundView : UiButtonView, IButtonView
    {
        [TabGroup("Components")]
        [SerializeField] private AudioSource _audioSource;

        private void Awake() =>
            _audioSource.loop = false;

        protected void OnEnable()
        {
            AddClickListener(OnClick);
            OnAfterEnable();
        }

        protected void OnDisable()
        {
            RemoveClickListener(OnClick);
            OnAfterDisable();
        }

        protected virtual void OnAfterEnable()
        {
        }

        protected virtual void OnAfterDisable()
        {
        }

        private void OnClick() =>
            _audioSource.Play();
    }
}