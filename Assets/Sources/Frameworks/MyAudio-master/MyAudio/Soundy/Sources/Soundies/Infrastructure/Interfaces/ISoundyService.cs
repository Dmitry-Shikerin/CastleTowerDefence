using Sources.Frameworks.MVPPassiveView.Controllers.Interfaces.ControllerLifetimes;
using UnityEngine;

namespace Sources.Frameworks.MyAudio_master.MyAudio.Soundy.Sources.Soundies.Infrastructure.Interfaces
{
    public interface ISoundyService : IInitialize, IDestroy
    {
        void Play(string databaseName, string soundName, Vector3 position);
        void Play(string databaseName, string soundName);
        void PlaySequence(string databaseName, string soundName);
        void StopSequence(string databaseName, string soundName);
        void Stop(string database, string sound);
    }
}