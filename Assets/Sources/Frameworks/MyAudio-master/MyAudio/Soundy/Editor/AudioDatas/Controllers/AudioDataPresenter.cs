using System;
using Doozy.Engine.Soundy;
using MyAudios.Soundy.Editor.AudioDatas.Presentation.View.Interfaces;
using MyAudios.Soundy.Editor.Presenters.Controllers;
using MyAudios.Soundy.Sources.DataBases.Domain.Data;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MyAudios.Soundy.Editor.AudioDatas.Controllers
{
    public class AudioDataPresenter : IPresenter
    {
        private AudioData _audioData;
        private SoundGroupData _soundGroupData;
        private IAudioDataView _view;
        private AudioSource _audioSource;

        public AudioDataPresenter(
            AudioData audioData,
            SoundGroupData soundGroupData,
            IAudioDataView view)
        {
            _audioData = audioData ?? throw new ArgumentNullException(nameof(audioData));
            _soundGroupData = soundGroupData ?? throw new ArgumentNullException(nameof(soundGroupData));
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _audioSource = Object.FindObjectOfType<AudioSource>();
        }

        public void Initialize()
        {
            _audioData.IsPlaying = false;
            _view.SetAudioClip(_audioData.AudioClip);
            InitializeSliderValue();
        }

        public void Dispose()
        {
            _audioData = null;
            _soundGroupData = null;
            _view = null;
            EditorApplication.update -= SetSliderValue;
        }

        public void PlaySound()
        {
            _view.StopAllSounds();
            
            _audioData.IsPlaying = true;
            _soundGroupData.IsPlaying = true;
            EditorApplication.update += SetSliderValue;
            _view.SetStopIcon();
            _soundGroupData.PlaySoundPreview(_audioSource, null, _audioData.AudioClip);
        }

        public void StopSound()
        {
            _audioData.IsPlaying = false;
            _soundGroupData.IsPlaying = false;
            EditorApplication.update -= SetSliderValue;
            _view.SetPlayIcon();
            _soundGroupData.StopSoundPreview(_audioSource);
            _view.SetSliderValue(0);
        }

        public void DeleteAudioData()
        {
            _soundGroupData.RemoveAudioData(_audioData);
            _view.Dispose();
        }

        public void SetAudioClip(AudioClip audioClip) =>
            _audioData.AudioClip = audioClip;

        public void ChangeSoundGroupState()
        {
            if (_audioData.IsPlaying == false)
                PlaySound();
            else
                StopSound();
        }

        private void SetSliderValue()
        {
            _view.SetSliderValue(_audioSource.time);
        }

        private void InitializeSliderValue()
        {
            float maxValue = 0;
            
            if (_audioData.AudioClip != null)
                maxValue = _audioData.AudioClip.length;
            
            _view.SetSliderValue(new Vector2(0, maxValue), 0);
        }
    }
}