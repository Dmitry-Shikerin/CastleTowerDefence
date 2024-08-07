using System.Collections.Generic;
using System.Linq;
using MyAudios.MyUiFramework.Utils;
using MyAudios.Soundy.Sources.AudioPoolers.Controllers;
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
        #region Static Properties

        /// <summary> Internal list of all the available controllers </summary>
        private static List<SoundyController> s_database = new List<SoundyController>();

        /// <summary> Global variable that keeps track if all controllers are paused or not </summary>
        private static bool s_pauseAllControllers;

        /// <summary> Global variable that keeps track if all controllers are muted or not </summary>
        private static bool s_muteAllControllers;

        /// <summary> Global toggle to pause / unpause all controllers </summary>
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

        /// <summary> Global toggle to mute / unmute all controllers </summary>
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

        #endregion
        
        #region Private Variables

        /// <summary> Internal variable that keeps a reference to the self Transform </summary>
        private Transform _transform;

        /// <summary> Internal variable that keeps a reference to the current follow target </summary>
        private Transform _followTarget;

        /// <summary> Internal variable that keeps a reference to the target AudioSource </summary>
        private AudioSource _audioSource;

        /// <summary> Internal variable that keeps track if this controller is in use or idle </summary>
        private bool _inUse;

        /// <summary> Internal variable that keeps track of the currently playing AudioClip play progress </summary>
        private float _playProgress;

        /// <summary> Internal variable that keeps track if this controller is paused or not </summary>
        private bool _isPaused;

        /// <summary> Internal variable that keeps track if this controller is muted or not </summary>
        private bool _isMuted;

        /// <summary> Internal variable that keeps track of when was the last time this controller was used </summary>
        private float _lastPlayedTime;

        /// <summary> Internal variable that keeps track if the Play() method has been called on this controller. It's set to FALSE when the Stop() method is called. </summary>
        private bool _isPlaying;

        /// <summary> Internal variable used to detect then Unity Pauses this controller's AudioSource (this happens on app switch for example and Unity does not give any info about this happening) </summary>
        private bool _autoPaused;

        private bool _muted;
        private bool _paused;

        #endregion
        
        #region Properties

        /// <summary> Target AudioSource component </summary>
        public AudioSource AudioSource
        {
            get => _audioSource;
            private set => _audioSource = value;
        }

        /// <summary> Keeps track if this controller is in use or idle </summary>
        public bool InUse
        {
            get => _inUse;
            private set => _inUse = value;
        }

        /// <summary> Keeps track of the currently playing AudioClip play progress </summary>
        public float PlayProgress
        {
            get => _playProgress;
            private set => _playProgress = value;
        }

        /// <summary> Keeps track if this controller is paused or not </summary>
        public bool IsPaused
        {
            get => _isPaused || s_pauseAllControllers;
            private set => _isPaused = value;
        }

        /// <summary> Keeps track if this controller is muted or not </summary>
        public bool IsMuted
        {
            get => _isMuted || MuteAllControllers;
            private set => _isMuted = value;
        }

        /// <summary> Keeps track of when was the last time this controller was used (info needed for the dynamic pooling system) </summary>
        public float LastPlayedTime
        {
            get => _lastPlayedTime;
            private set => _lastPlayedTime = value;
        }

        /// <summary> Returns the duration since this controller has been used last </summary>
        public float IdleDuration => Time.realtimeSinceStartup - LastPlayedTime;

        #endregion
        
        #region Unity Methods

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
                    AudioSource.Pause();
                
                if (!IsPaused) 
                    AudioSource.UnPause();
                
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

        #endregion

        #region Public Methods

        /// <summary> Stop playing and destroy the GameObject this controller is attached to </summary>
        public void Kill()
        {
            Stop();
            Destroy(gameObject);
        }

        /// <summary> Mute the target AudioSource </summary>
        public void Mute()
        {
            IsMuted = true;
        }

        /// <summary> Pause the target AudioSource </summary>
        public void Pause()
        {
            IsPaused = true;
        }

        /// <summary> Start Play on the target AudioSource </summary>
        public void Play()
        {
            InUse = true;
            IsPaused = false;
            _isPlaying = true;
            AudioSource.Play();
        }

        /// <summary> Set a follow target Transform that this controller needs to follow while playing </summary>
        /// <param name="followTarget"> The target Transform </param>
        public void SetFollowTarget(Transform followTarget) =>
            _followTarget = followTarget;

        /// <summary> Set an output AudioMixerGroup to the target AudioSource of this controller </summary>
        /// <param name="outputAudioMixerGroup"> Target output AudioMixerGroup </param>
        public void SetOutputAudioMixerGroup(AudioMixerGroup outputAudioMixerGroup)
        {
            if (outputAudioMixerGroup == null)
                return;
            
            AudioSource.outputAudioMixerGroup = outputAudioMixerGroup;
        }

        /// <summary> Set the position in world space from where this controller will be playing from </summary>
        /// <param name="position"> The new position </param>
        public void SetPosition(Vector3 position) =>
            _transform.position = position;

        /// <summary> Set the given settings to the target AudioSource </summary>
        /// <param name="clip"> The AudioClip to play </param>
        /// <param name="volume"> The volume of the audio source (0.0 to 1.0) </param>
        /// <param name="pitch"> The pitch of the audio source </param>
        /// <param name="loop"> Is the audio clip looping? </param>
        /// <param name="spatialBlend"> Sets how much this AudioSource is affected by 3D spatialisation calculations (attenuation, doppler etc). 0.0 makes the sound full 2D, 1.0 makes it full 3D </param>
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

        /// <summary> Stop the target AudioSource from playing </summary>
        public void Stop()
        {
            Unpause();
            Unmute();
            AudioSource.Stop();
            _isPlaying = false;
            ResetController();
            SoundyPooler.PutControllerInPool(this);
        }

        /// <summary> Unmute the target AudioSource if it was previously muted </summary>
        public void Unmute()
        {
            IsMuted = false;
        }

        /// <summary> Unpause the target AudioSource if it was previously paused </summary>
        public void Unpause()
        {
            IsPaused = false;
        }

        #endregion

        #region Private Methods

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

        #endregion

        #region Static Methods

        /// <summary> Create a new SoundyController in the current scene and get a reference to it </summary>
        public static SoundyController GetController()
        {
            SoundyController controller = new GameObject(
                "SoundyController", 
                typeof(AudioSource), 
                typeof(SoundyController)).GetComponent<SoundyController>();
            
            return controller;
        }

        /// <summary> Stop all controllers from playing and destroy the GameObjects they are attached to </summary>
        public static void KillAll()
        {
            RemoveNullControllersFromDatabase();
            
            foreach (SoundyController controller in s_database)
                controller.Kill();
        }

        /// <summary> Mute all the controllers </summary>
        public static void MuteAll()
        {
            RemoveNullControllersFromDatabase();
            MuteAllControllers = true;
        }

        /// <summary> Pause all the controllers that are currently playing </summary>
        public static void PauseAll()
        {
            RemoveNullControllersFromDatabase();
            PauseAllControllers = true;
        }

        /// <summary> Remove any null controller references from the database </summary>
        public static void RemoveNullControllersFromDatabase() =>
            s_database = s_database.Where(sc => sc != null).ToList();

        /// <summary> Stop all the controllers that are currently playing </summary>
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

        /// <summary> Unmute all the controllers that were previously muted </summary>
        public static void UnmuteAll()
        {
            RemoveNullControllersFromDatabase();
            MuteAllControllers = false;
        }

        /// <summary> Unpause all the controllers that were previously paused </summary>
        public static void UnpauseAll() =>
            PauseAllControllers = false;

        #endregion
    }
}