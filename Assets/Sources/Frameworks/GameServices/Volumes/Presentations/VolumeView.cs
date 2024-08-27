using System;
using Doozy.Runtime.UIManager.Components;
using Sources.Frameworks.GameServices.Volumes.Domain.Constant;
using Sources.Frameworks.GameServices.Volumes.Domain.Models.Implementation;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using UnityEngine;

namespace Sources.Frameworks.GameServices.Volumes.Presentations
{
    public class VolumeView : View
    {
        [SerializeField] private UISlider _slider;
        [SerializeField] private UIToggle _toggle;
        [SerializeField] private UIButton _leftArrow;
        [SerializeField] private UIButton _rightArrow;
        
        private Volume _volume;
        private bool _isSliderEnabled = true;

        private void OnEnable()
        {
            _toggle.OnToggleOnCallback.Event.AddListener(EnableSlider);
            _toggle.OnToggleOffCallback.Event.AddListener(DisableSlider);
            _leftArrow.onClickEvent.AddListener(ReduceSliderValue);
            _rightArrow.onClickEvent.AddListener(IncreaseSliderValue);
        }

        private void OnDisable()
        {
            _toggle.OnToggleOnCallback.Event.RemoveListener(EnableSlider);
            _toggle.OnToggleOffCallback.Event.RemoveListener(DisableSlider);
            _leftArrow.onClickEvent.RemoveListener(ReduceSliderValue);
            _rightArrow.onClickEvent.RemoveListener(IncreaseSliderValue);
        }

        public void Construct(Volume volume)
        {
            _volume = volume ?? throw new ArgumentNullException(nameof(volume));
            SetSliderValue();
            SetToggleValue();
        }

        private void SetSliderValue()
        { 
            _slider.value = _volume.VolumeValue;
        }

        private void SetToggleValue()
        {
            if (_volume.IsVolumeMuted)
            {
                _toggle.isOn = false;
                DisableSlider();
                
                return;
            }
            
            _toggle.isOn = true;
            EnableSlider();
        }

        private void EnableSlider()
        {
            _slider.interactable = true;
            _isSliderEnabled = true;
            _volume.IsVolumeMuted = false;
        }

        private void DisableSlider()
        {
            _slider.interactable = false;
            _isSliderEnabled = false;
            _volume.IsVolumeMuted = true;
        }

        private void ReduceSliderValue()
        {
            if (_isSliderEnabled == false || _slider.value == 0)
                return;

            _slider.value -= VolumeConst.VolumeValuePerStep;
            _volume.VolumeValue = _slider.value;
        }

        private void IncreaseSliderValue()
        {
            if (_isSliderEnabled == false || _slider.value == 1)
                return;

            _slider.value += VolumeConst.VolumeValuePerStep;
            _volume.VolumeValue = _slider.value;
        }
    }
}