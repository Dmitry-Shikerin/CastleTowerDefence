using Doozy.Runtime.UIManager.Components;
using MyAudios.Soundy.Sources.DataBases.Domain.Data;
using MyAudios.Soundy.Sources.Managers.Controllers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MyAudios.MyUiFramework.Utils.UiButtonComponents.Presentation
{
    [RequireComponent(typeof(UIButton))]
    public class UiButtonToSoundy : MonoBehaviour
    {
        [SerializeField] private UIButton _button;
        [SerializeField] private SoundyData _soundyData;

        private void OnEnable()
        {
            _button.onClickEvent.AddListener(OnClick);
        }

        private void OnDisable()
        {
            _button.onClickEvent.RemoveListener(OnClick);
        }

        private void OnClick() =>
            SoundyManager.Play(_soundyData, true);
        
        [OnInspectorInit]
        private void SetButton() =>
            _button = GetComponent<UIButton>();
    }
}