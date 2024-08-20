using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using MyAudios.Soundy.Sources.AudioControllers.Controllers;
using MyAudios.Soundy.Sources.Managers.Controllers;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.Frameworks.GameServices.Pauses.Services.Interfaces;
using Sources.Frameworks.GameServices.Volumes.Domain.Models.Implementation;
using Sources.InfrastructureInterfaces.Services.Repositories;
using UnityEngine;

namespace MyAudios.MyUiFramework.Utils.Soundies.Infrastructure
{
    public class SoundyService : ISoundyService
    {
        private readonly IPauseService _pauseService;
        private readonly IEntityRepository _entityRepository;
        private readonly Dictionary<string, Dictionary<string, CancellationTokenSource>> _tokens;

        private Volume _musicVolume;
        private Volume _soundsVolume;

        private string _musicSoundName;

        public SoundyService(
            IPauseService pauseService,
            IEntityRepository entityRepository)
        {
            _pauseService = pauseService ?? throw new ArgumentNullException(nameof(pauseService));
            _entityRepository = entityRepository ?? throw new ArgumentNullException(nameof(entityRepository));
            _tokens = new Dictionary<string, Dictionary<string, CancellationTokenSource>>();
        }

        public void Initialize()
        {
            _musicVolume = _entityRepository.Get<Volume>(ModelId.MusicVolume);
            _soundsVolume = _entityRepository.Get<Volume>(ModelId.SoundsVolume);
            OnSoundsVolumeChanged();
            _pauseService.PauseSoundActivated += OnPauseSoundActivated;
            _pauseService.ContinueSoundActivated += OnContinueSoundActivated;
            _musicVolume.VolumeChanged += OnMusicVolumeChanged;
            _soundsVolume.VolumeChanged += OnSoundsVolumeChanged;
            _musicVolume.VolumeMuted += OnMusicVolumeMuted;
            _soundsVolume.VolumeMuted += OnSoundsVolumeMuted;
        }

        public void Destroy()
        {
            _pauseService.PauseSoundActivated -= OnPauseSoundActivated;
            _pauseService.ContinueSoundActivated -= OnContinueSoundActivated;
            _musicVolume.VolumeChanged -= OnMusicVolumeChanged;
            _soundsVolume.VolumeChanged -= OnSoundsVolumeChanged;
            _musicVolume.VolumeMuted -= OnMusicVolumeMuted;
            _soundsVolume.VolumeMuted -= OnSoundsVolumeMuted;
        }

        public void Play(string databaseName, string soundName, Vector3 position) =>
            SoundyManager.Play(databaseName, soundName, position);

        public void Play(string databaseName, string soundName)
        {
            SoundyManager.Play(databaseName, soundName);
            SoundyManager.SetVolume(soundName, _soundsVolume.VolumeValue);
        }

        public async void PlaySequence(string databaseName, string soundName)
        {
            _musicSoundName = soundName;
            SoundyManager.PlaySequence(databaseName, soundName, _musicVolume);
        }

        public void StopSequence(string databaseName, string soundName) =>
            SoundyManager.StopSequence(databaseName, soundName);

        private void OnPauseSoundActivated() =>
            SoundyManager.PauseAllControllers();

        private void OnContinueSoundActivated() =>
            SoundyManager.UnpauseAllControllers();

        private void OnSoundsVolumeChanged()
        {
            SoundyManager.SetVolumes(
                _musicVolume.VolumeValue,
                _soundsVolume.VolumeValue);   
        }

        private void OnMusicVolumeChanged()
        {
            SoundyController
                .GetControllerByName(_musicSoundName)
                .AudioSource.volume = _musicVolume.VolumeValue;
        }

        private void OnSoundsVolumeMuted()
        {
            SoundyManager.SetMutes(
                _musicVolume.IsVolumeMuted,
                _soundsVolume.IsVolumeMuted);
        }

        private void OnMusicVolumeMuted()
        {
            SoundyController
                .GetControllerByName(_musicSoundName)
                .AudioSource.mute = _musicVolume.IsVolumeMuted;
            OnSoundsVolumeMuted();
        }
    }
}