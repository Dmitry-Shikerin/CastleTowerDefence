using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using MyAudios.Soundy.Sources.AudioControllers.Controllers;
using MyAudios.Soundy.Sources.Managers.Controllers;
using UnityEngine;

namespace MyAudios.MyUiFramework.Utils.Soundies.Infrastructure
{
    public class SoundyService : ISoundyService
    {
        private Dictionary<string, Dictionary<string, CancellationTokenSource>> _tokens = 
            new Dictionary<string, Dictionary<string, CancellationTokenSource>>();
        
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