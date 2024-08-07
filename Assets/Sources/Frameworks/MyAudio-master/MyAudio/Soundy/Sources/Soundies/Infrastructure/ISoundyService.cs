using UnityEngine;

namespace MyAudios.MyUiFramework.Utils.Soundies.Infrastructure
{
    public interface ISoundyService
    {
        void Play(string databaseName, string soundName, Vector3 position);
        void Play(string databaseName, string soundName);
    }
}