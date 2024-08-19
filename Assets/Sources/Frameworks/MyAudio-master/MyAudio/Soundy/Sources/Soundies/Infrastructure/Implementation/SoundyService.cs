using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using MyAudios.Soundy.Sources.AudioControllers.Controllers;
using MyAudios.Soundy.Sources.Managers.Controllers;
using Sources.Frameworks.GameServices.Pauses.Services.Interfaces;
using UnityEngine;

namespace MyAudios.MyUiFramework.Utils.Soundies.Infrastructure
{
    public class SoundyService : ISoundyService
    {
        private readonly IPauseService _pauseService;

        private Dictionary<string, Dictionary<string, CancellationTokenSource>> _tokens = 
            new Dictionary<string, Dictionary<string, CancellationTokenSource>>();

        public void Initialize()
        {
            _pauseService.PauseSoundActivated += OnPauseSoundActivated;
            _pauseService.ContinueSoundActivated += OnContinueSoundActivated;
        }

        private void OnPauseSoundActivated() =>
            SoundyManager.PauseAllControllers();

        private void OnContinueSoundActivated() =>
            SoundyManager.UnpauseAllControllers();

        public void Destroy()
        {
            _pauseService.PauseSoundActivated -= OnPauseSoundActivated;
            _pauseService.ContinueSoundActivated -= OnContinueSoundActivated;
        }

        public SoundyService(IPauseService pauseService)
        {
            _pauseService = pauseService ?? throw new ArgumentNullException(nameof(pauseService));
        }

        public void Play(string databaseName, string soundName, Vector3 position) =>
            SoundyManager.Play(databaseName, soundName, position);

        public void Play(string databaseName, string soundName)
        {
            SoundyManager.Play(databaseName, soundName);
            SoundyManager.SetVolume(soundName, 0.2f);
        }

        public async void PlaySequence(string databaseName, string soundName) =>
            SoundyManager.PlaySequence(databaseName, soundName);

        public void StopSequence(string databaseName, string soundName) =>
            SoundyManager.StopSequence(databaseName, soundName);
    }
}