using Sirenix.OdinInspector;
using Sources.Frameworks.GameServices.Cameras.Domain;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using UnityEngine;
using UnityEngine.Playables;

namespace Sources.Frameworks.GameServices.Cameras.Presentation.Implementation
{
    public class PlayableDirectorView : View
    {
        [SerializeField] private DirectorId _directorId;
        [SerializeField] private PlayableDirector _director;
        
        public DirectorId DirectorId => _directorId;
        public PlayState PlayState => _director.state;
        
        private void Awake() =>
            _director.playOnAwake = false;

        public void Play() =>
            _director.Play();

        public void Stop() =>
            _director.Stop();
        
        public void Pause() =>
            _director.Pause();

        [OnInspectorInit]
        private void SetDirector()
        {
            if (_director != null)
                return;
            
            _director = GetComponent<PlayableDirector>();
        }
    }
}