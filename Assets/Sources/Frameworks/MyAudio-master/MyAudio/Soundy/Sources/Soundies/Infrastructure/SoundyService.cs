using MyAudios.Soundy.Sources.Managers.Controllers;
using UnityEngine;

namespace MyAudios.MyUiFramework.Utils.Soundies.Infrastructure
{
    public class SoundyService : ISoundyService
    {
        public void Play(string databaseName, string soundName, Vector3 position) =>
            SoundyManager.Play(databaseName, soundName, position);

        public void Play(string databaseName, string soundName) =>
            SoundyManager.Play(databaseName, soundName);
    }
}