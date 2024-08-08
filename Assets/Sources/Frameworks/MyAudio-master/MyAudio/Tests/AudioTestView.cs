using System;
using MyAudios.Soundy.Sources.Managers.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace MyAudios.Tests
{
    public class AudioTestView : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            SoundyManager.Play("General", "Button", Vector3.zero);
        }
    }
}