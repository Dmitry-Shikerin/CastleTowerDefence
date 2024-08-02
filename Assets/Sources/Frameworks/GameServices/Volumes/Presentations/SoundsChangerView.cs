using System;
using Doozy.Runtime.UIManager.Components;
using Sources.Frameworks.GameServices.Volumes.Domain.Constant;
using Sources.Frameworks.GameServices.Volumes.Domain.Models.Implementation;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using UnityEngine;

namespace Sources.Frameworks.GameServices.Volumes.Presentations
{
    public class SoundsChangerView : View
    {
        [SerializeField] private UISlider _slider;
        [SerializeField] private UIToggle toggle;
        [SerializeField] private UIButton _leftArrow;
        [SerializeField] private UIButton _rightArrow;
        
        private Volume _volume;
        private bool _isSliderEnabled = true;

        private void OnEnable()
        {
            toggle.OnToggleOnCallback.Event.AddListener(EnableSlider);
            toggle.OnToggleOffCallback.Event.AddListener(DisableSlider);
            _leftArrow.onClickEvent.AddListener(ReduceSliderValue);
            _rightArrow.onClickEvent.AddListener(IncreaseSliderValue);
        }

        private void OnDisable()
        {
            toggle.OnToggleOnCallback.Event.RemoveListener(EnableSlider);
            toggle.OnToggleOffCallback.Event.RemoveListener(DisableSlider);
            _leftArrow.onClickEvent.RemoveListener(ReduceSliderValue);
            _rightArrow.onClickEvent.RemoveListener(IncreaseSliderValue);
        }

        public void Construct(Volume volume)
        {
            _volume = volume ?? throw new ArgumentNullException(nameof(volume));
            
            SetSliderValue();
        }

        private void SetSliderValue()
        {
            if (_volume.SoundsVolume > 0)
            {
                _slider.value = _volume.SoundsVolume;
                toggle.isOn = true;
                
                EnableSlider();
                
                return;
            }
            
            DisableSlider();
            toggle.isOn = false;
        }

        private void EnableSlider()
        {
            _slider.interactable = true;
            _isSliderEnabled = true;
            _volume.IsSoundsMuted = false;
        }

        private void DisableSlider()
        {
            _slider.interactable = false;
            _isSliderEnabled = false;
            _volume.IsSoundsMuted = true;
        }

        private void ReduceSliderValue()
        {
            if (_isSliderEnabled == false || _slider.value == 0)
                return;

            _slider.value -= VolumeConst.VolumeValuePerStep;
            _volume.SoundsVolume = _slider.value;
        }

        private void IncreaseSliderValue()
        {
            if (_isSliderEnabled == false || _slider.value == 1)
                return;

            _slider.value += VolumeConst.VolumeValuePerStep;
            _volume.SoundsVolume = _slider.value;
        }
    }
}