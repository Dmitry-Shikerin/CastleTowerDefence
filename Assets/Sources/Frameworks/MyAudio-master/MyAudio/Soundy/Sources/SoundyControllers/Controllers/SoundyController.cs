using System.Collections.Generic;
using System.Linq;
using MyAudios.MyUiFramework.Utils;
using Sirenix.Utilities;
using Sources.Frameworks.MyAudio_master.MyAudio.Soundy.Sources.AudioPoolers.Controllers;
using UnityEngine;
using UnityEngine.Audio;

namespace MyAudios.Soundy.Sources.AudioControllers.Controllers
{
    /// <inheritdoc />
    /// <summary>
    ///     This is an audio controller used by the Soundy system to play sounds. Each sound has its own controller that handles it.
    ///     Every SoundyController is also added to the SoundyPooler to work seamlessly with the dynamic sound pooling system.
    /// </summary>
    [DefaultExecutionOrder(SoundyExecutionOrder.SoundyController)]
    public class SoundyController : MonoBehaviour
    {
        private static List<SoundyController> s_database = new List<SoundyController>();
        private static bool s_pauseAllControllers;
        private static bool s_muteAllControllers;

        public static bool PauseAllControllers
        {
            get => s_pauseAllControllers;
            set
            {
                s_pauseAllControllers = value;
                
                if (s_pauseAllControllers) 
                    return;
                
                RemoveNullControllersFromDatabase();
                
                foreach (SoundyController controller in s_database)
                    controller.Unpause();
            }
        }

        public static bool MuteAllControllers
        {
            get => s_muteAllControllers;
            set
            {
                s_muteAllControllers = value;
                
                if (s_muteAllControllers) 
                    return;
                
                RemoveNullControllersFromDatabase();
                
                foreach (SoundyController controller in s_database)
                    controller.Unmute();
            }
        }
        
        private Transform _transform;
        private Transform _followTarget;
        private AudioSource _audioSource;
        private bool _inUse;
        private float _playProgress;
        private bool _isPaused;
        private bool _isMuted;
        private float _lastPlayedTime;
        private bool _isPlaying;
        private bool _autoPaused;
        private bool _muted;
        private bool _paused;
        private string _name;
        private float _savedClipTime;

        public bool IsPlaying => _isPlaying;
        
        public string Name
        {
            get => _name;
            set => _name = value;
        }
        
        public AudioSource AudioSource
        {
            get => _audioSource;
            private set => _audioSource = value;
        }

        public bool InUse
        {
            get => _inUse;
            private set => _inUse = value;
        }

        public float PlayProgress
        {
            get => _playProgress;
            private set => _playProgress = value;
        }

        public bool IsPaused
        {
            get => _isPaused || s_pauseAllControllers;
            private set => _isPaused = value;
        }

        public bool IsMuted
        {
            get => _isMuted || MuteAllControllers;
            private set => _isMuted = value;
        }

        public float LastPlayedTime
        {
            get => _lastPlayedTime;
            private set => _lastPlayedTime = value;
        }

        public float IdleDuration => Time.realtimeSinceStartup - LastPlayedTime;
        

        private void Reset() =>
            ResetController();

        private void Awake()
        {
            s_database.Add(this);
            _transform = transform;
            AudioSource = gameObject.GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
            ResetController();
        }

        private void OnDestroy() =>
            s_database.Remove(this);

        private void Update()
        {
            if (IsMuted || IsPaused || AudioSource.isPlaying)
                UpdateLastPlayedTime();
            
            if (IsMuted != _muted)
            {
                AudioSource.mute = IsMuted;
                _muted = IsMuted;
            }

            if (IsPaused != _paused)
            {
                if (IsPaused && AudioSource.isPlaying)
                {
                    _savedClipTime = AudioSource.time;
                    AudioSource.Pause();
                }

                if (IsPaused == false)
                {
                    AudioSource.time = _savedClipTime;
                    AudioSource.UnPause();
                } 
                
                _paused = IsPaused;
            }

            UpdatePlayProgress();

            if (PlayProgress >= 1f) //check if the sound finished playing
            {
                Stop();
                PlayProgress = 0;
                
                return;
            }

            _autoPaused = InUse && _isPlaying && !AudioSource.isPlaying && PlayProgress > 0;

            if (InUse && !_autoPaused && !AudioSource.isPlaying && !IsPaused && !IsMuted) //second check if the sound finished playing
            {
                Stop();
                
                return;
            }

            FollowTarget();
        }

        public void Kill()
        {
            Stop();
            Destroy(gameObject);
        }

        public void Mute()
        {
            IsMuted = true;
        }

        public void Pause()
        {
            IsPaused = true;
        }

        public void Play()
        {
            InUse = true;
            IsPaused = false;
            _isPlaying = true;
            AudioSource.Play();
        }

        public void SetFollowTarget(Transform followTarget) =>
            _followTarget = followTarget;

        public void SetOutputAudioMixerGroup(AudioMixerGroup outputAudioMixerGroup)
        {
            if (outputAudioMixerGroup == null)
                return;
            
            AudioSource.outputAudioMixerGroup = outputAudioMixerGroup;
        }

        public void SetPosition(Vector3 position) =>
            _transform.position = position;

        public void SetSourceProperties(AudioClip clip, float volume, float pitch, bool loop, float spatialBlend)
        {
            if (clip == null)
            {
                Stop();
                return;
            }

            AudioSource.clip = clip;
            AudioSource.volume = volume;
            AudioSource.pitch = pitch;
            AudioSource.loop = loop;
            AudioSource.spatialBlend = spatialBlend;
        }

        public void Stop()
        {
            Unpause();
            Unmute();
            AudioSource.Stop();
            _isPlaying = false;
            ResetController();
            SoundyPooler.PutControllerInPool(this);
        }

        public void Unmute()
        {
            IsMuted = false;
        }

        public void Unpause()
        {
            IsPaused = false;
        }

        private void FollowTarget()
        {
            if (_followTarget == null) 
                return;
            
            _transform.position = _followTarget.position;
        }

        private void ResetController()
        {
            InUse = false;
            IsPaused = false;
            _followTarget = null;
            UpdateLastPlayedTime();
        }

        private void UpdateLastPlayedTime() =>
            LastPlayedTime = Time.realtimeSinceStartup;

        private void UpdatePlayProgress()
        {
            if (AudioSource == null)
                return;
            
            if (AudioSource.clip == null)
                return;
            
            PlayProgress = Mathf.Clamp01(AudioSource.time / AudioSource.clip.length);
        }

        public static SoundyController CreateController()
        {
            SoundyController controller = new GameObject(
                "SoundyController", 
                typeof(AudioSource), 
                typeof(SoundyController)).GetComponent<SoundyController>();
            
            return controller;
        }

        public static void Pause(string soundName)
        {
            s_database
                .Where(controller => controller.Name == soundName)
                .ForEach(controller => controller.Pause());
        }
        
        public static void Unpause(string soundName)
        {
            s_database
                .Where(controller => controller.Name == soundName)
                .ForEach(controller => controller.Unpause());
        }

        public static SoundyController GetControllerByName(string name) =>
            s_database.First(controller => controller.Name == name);

        public static void KillAll()
        {
            RemoveNullControllersFromDatabase();
            
            foreach (SoundyController controller in s_database)
                controller.Kill();
        }

        public static void MuteAll()
        {
            RemoveNullControllersFromDatabase();
            MuteAllControllers = true;
        }

        public static void PauseAll()
        {
            RemoveNullControllersFromDatabase();
            PauseAllControllers = true;
        }

        public static void RemoveNullControllersFromDatabase() =>
            s_database = s_database.Where(sc => sc != null).ToList();

        public static void StopAll()
        {
            RemoveNullControllersFromDatabase();
            
            foreach (SoundyController controller in s_database)
            {
                if (!controller.AudioSource.isPlaying)
                    return;
                
                controller.Stop();
            }
        }

        public static void UnmuteAll()
        {
            RemoveNullControllersFromDatabase();
            MuteAllControllers = false;
        }

        public static void UnpauseAll() =>
            PauseAllControllers = false;
        
        public static void Stop(string databaseName, string soundName)
        {
            s_database
                .Where(controller => controller.Name == soundName)
                .ForEach(controller => controller.Stop());
        }
        
        public static void SetVolume(string soundName, float volume)
        {
            s_database
                .Where(controller => controller.Name == soundName)
                .ForEach(controller => controller.AudioSource.volume = volume);
        }
    }
}