using System;
using Doozy.Editor.EditorUI;
using Doozy.Editor.EditorUI.Components;
using MyAudios.Soundy.Editor.AudioDatas.Controllers;
using MyAudios.Soundy.Editor.AudioDatas.Presentation.Controlls;
using MyAudios.Soundy.Editor.AudioDatas.Presentation.View.Interfaces;
using MyAudios.Soundy.Editor.SoundGroupDatas.Presentation.Views.Interfaces;
using MyAudios.Soundy.Editor.SoundGroups.Presentation.Views.Interfaces;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.AudioDatas.Presentation.View.Implementation
{
    public class AudioDataView : IAudioDataView
    {
        private AudioDataPresenter _presenter;
        private AudioDataVisualElement _visualElement;
        private FluidButton _playButton;
        private FluidButton _deleteButton;
        private FluidRangeSlider _topSlider;
        private AudioClip _audioClip;
        private bool _isPlaying;
        private ObjectField _objectField;
        private Label _label;
        private ISoundGroupDataView _soundGroupDataView;

        public VisualElement Root { get; private set; }

        public void Construct(AudioDataPresenter audioDataPresenter)
        {
            _presenter = audioDataPresenter ?? throw new ArgumentNullException(nameof(audioDataPresenter));
            
            CreateView();
            Initialize();
        }

        public void CreateView()
        {
            _visualElement = new AudioDataVisualElement();
            Root = _visualElement;
            _deleteButton = _visualElement.DeleteButton;
            _playButton = _visualElement.PlayButton;
            _topSlider = _visualElement.Slider;
            _objectField = _visualElement.ObjectField;
        }

        public void Initialize()
        {
            _playButton.SetOnClick(ChangeSoundGroupState);
            _deleteButton.SetOnClick(_presenter.DeleteAudioData);
            _objectField.RegisterValueChangedCallback((value) => 
                _presenter.SetAudioClip(value.newValue as AudioClip));
            _presenter.Initialize();
        }

        private void ChangeSoundGroupState() =>
            _presenter.ChangeSoundGroupState();

        public void StopPlaySound()
        {
            _presenter.StopSound();
        }

        public void SetSliderValue(float value) =>
            _topSlider.slider.value = value;

        public void SetSliderValue(Vector2 minMaxValue, float value)
        {
            _topSlider.slider.lowValue = minMaxValue.x;
            _topSlider.slider.highValue = minMaxValue.y;
            _topSlider.slider.value = value;
        }

        public void SetStopIcon() =>
            _playButton.SetIcon(EditorSpriteSheets.EditorUI.Icons.Stop);

        public void SetPlayIcon() =>
            _playButton.SetIcon(EditorSpriteSheets.EditorUI.Icons.Play);

        public void SetAudioClip(AudioClip audioClip) =>
            _objectField.SetValueWithoutNotify(audioClip);

        public void StopAllSounds() =>
            _soundGroupDataView.StopAllAudioData();

        public void SetSoundGroupData(ISoundGroupDataView soundGroupDataView)
        {
            _soundGroupDataView = soundGroupDataView ?? throw new ArgumentNullException(nameof(soundGroupDataView));
        }

        public void Dispose()
        {
            _playButton?.Dispose();
            _deleteButton?.Dispose();
            Root.RemoveFromHierarchy();
            _presenter?.Dispose();
        }
    }
}