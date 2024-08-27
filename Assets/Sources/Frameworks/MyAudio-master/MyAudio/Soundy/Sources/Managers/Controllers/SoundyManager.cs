using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Doozy.Engine.Soundy;
using MyAudios.MyUiFramework.Utils;
using MyAudios.Soundy.Sources.AudioControllers.Controllers;
using MyAudios.Soundy.Sources.DataBases.Domain.Data;
using MyAudios.Soundy.Sources.Managers.Domain.Constants;
using MyAudios.Soundy.Sources.Settings.Domain.Configs;
using MyAudios.Soundy.Sources.SoundSources.Enums;
using Sirenix.Utilities;
using Sources.Frameworks.GameServices.Volumes.Domain.Models.Implementation;
using Sources.Frameworks.MyAudio_master.MyAudio.Soundy.Sources.AudioPoolers.Controllers;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

namespace MyAudios.Soundy.Sources.Managers.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Central component of the Soundy system that binds all the sound sub-systems together.
    /// It gets the SoundGroupData references from the SoundyDatabase and passes them to the SoundyPooler, that in turn manages and uses SoundyControllers to play the sounds. 
    /// </summary>
    [AddComponentMenu(SoundyManagerConstant.SoundyManagerAddComponentMenuMenuName,
        SoundyManagerConstant.SoundyManagerAddComponentMenuOrder)]
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(SoundyExecutionOrder.SoundyManager)]
    public class SoundyManager : MonoBehaviour
    {
#if UNITY_EDITOR
        [MenuItem(SoundyManagerConstant.SoundyManagerMenuItemItemName, false,
            SoundyManagerConstant.SoundyManagerMenuItemPriority)]
        private static void CreateComponent(MenuCommand menuCommand)
        {
            SoundyManager addToScene = AddToScene(true);
        }
#endif
        protected SoundyManager()
        {
        }

        private static SoundyManager s_instance;
        private static float s_musicVolume;
        private static float s_soundVolume;        
        private static bool s_isMusicVolumeMuted;
        private static bool s_isSoundVolumeMuted;

        public static SoundyManager Instance
        {
            get
            {
                if (s_instance != null)
                    return s_instance;

                if (ApplicationIsQuitting)
                    return null;

                s_instance = FindObjectOfType<SoundyManager>();
                if (s_instance == null)
                    DontDestroyOnLoad(AddToScene(false).gameObject);

                return s_instance;
            }
        }

        private static Dictionary<string, Dictionary<string, CancellationTokenSource>> _tokens =
            new Dictionary<string, Dictionary<string, CancellationTokenSource>>();

        /// <summary> Internal variable used as a flag when the application is quitting </summary>
        private static bool ApplicationIsQuitting = false;

        /// <summary> Internal variable that keeps track if this class has been initialized </summary>
        private static bool s_initialized;

        /// <summary> Internal variable that keeps a reference to the SoundyPooler component attached to this GameObject </summary>
        private static SoundyPooler s_pooler;


        /// <summary> Returns a reference to the SoundyPooler component on this GameObject. If one does not exist, it gets added. </summary>
        public static SoundyPooler Pooler
        {
            get
            {
                if (s_pooler != null)
                    return s_pooler;

                s_pooler = Instance.gameObject.GetComponent<SoundyPooler>();

                if (s_pooler == null)
                    s_pooler = Instance.gameObject.AddComponent<SoundyPooler>();

                return s_pooler;
            }
        }
        
        public static SoundyDatabase Database => SoundySettings.Database;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void RunOnStart()
        {
            ApplicationIsQuitting = false;
            s_initialized = false;
            s_pooler = null;
        }

        private void Awake() =>
            s_initialized = true;
        
        public static SoundyManager AddToScene(bool selectGameObjectAfterCreation = false) =>
            MyUtils.AddToScene<SoundyManager>(
                SoundyManagerConstant.SoundyManagerGameObjectName, true, selectGameObjectAfterCreation);

        public static SoundyController CreateController() =>
            SoundyController.CreateController();

        public static string GetSoundDatabaseFilename(string databaseName) =>
            "SoundDatabase_" + databaseName.Trim();

        public static void Init()
        {
            if (s_initialized || s_instance != null)
                return;

            s_instance = Instance;

            for (int i = 0; i < SoundyPooler.MinimumNumberOfControllers + 1; i++)
                SoundyPooler.GetControllerFromPool().Stop();
        }

        public static void Pause(string soundName)
        {
            SoundyController.Pause(soundName);
        }

        public static void UnPause(string soundName)
        {
            SoundyController.Unpause(soundName);
        }

        public static void ClearTokens()
        {
            _tokens.Values
                .ForEach(collection => collection.Values
                    .ForEach(token => token.Cancel()));
            _tokens.Clear();
        }

        public static void SetVolumes(float musicVolume, float soundVolume)
        {
            s_musicVolume = musicVolume;
            s_soundVolume = soundVolume;
        }

        public static void SetMutes(bool isMusicMuted, bool isSoundMuted)
        {
            s_isMusicVolumeMuted = isMusicMuted;
            s_isSoundVolumeMuted = isSoundMuted;
        }

        public static void KillAllControllers() =>
            SoundyController.KillAll();

        public static void MuteAllControllers() =>
            SoundyController.MuteAll();

        public static void MuteAllSounds()
        {
            MuteAllControllers();
#if dUI_MasterAudio
            DarkTonic.MasterAudio.MasterAudio.MuteEverything();
#endif
        }

        public static void PauseAllControllers() =>
            SoundyController.PauseAll();

        public static void PauseAllSounds()
        {
            PauseAllControllers();
#if dUI_MasterAudio
            DarkTonic.MasterAudio.MasterAudio.PauseEverything();
#endif
        }

        public static void SetVolume(string soundName, float volume) =>
            SoundyController.SetVolume(soundName, volume);

        public static async void PlaySequence(
            string databaseName, 
            string soundName,
            Volume musicVolume)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            if (_tokens.ContainsKey(databaseName) == false)
                _tokens[databaseName] = new Dictionary<string, CancellationTokenSource>();

            _tokens[databaseName].Add(soundName, cancellationTokenSource);

            try
            {
                while (cancellationTokenSource.Token.IsCancellationRequested == false)
                {
                    SoundyController soundyController = Play(databaseName, soundName);
                    SetVolume(soundName, musicVolume.VolumeValue);
                    AudioSource audioSource = soundyController.AudioSource;
                    audioSource.mute = musicVolume.IsVolumeMuted;

                    // await UniTask.WaitUntil(
                    //     () => audioSource.time + 0.1f > audioSource.clip.length,
                    //     cancellationToken: cancellationTokenSource.Token);
                    await UniTask.WaitUntil(
                        () => soundyController.IsPlaying == false);
                }
            }
            catch (OperationCanceledException)
            {
                Stop(databaseName, soundName);
            }
        }

        public static void StopSequence(string databaseName, string soundName)
        {
            Dictionary<string, CancellationTokenSource> tokens = _tokens[databaseName];
            tokens[soundName].Cancel();
            Stop(databaseName, soundName);
        }

        public static SoundyController Play(string databaseName, string soundName, Vector3 position)
        {
            if (s_initialized == false)
                s_instance = Instance;

            if (Database == null)
                return null;

            if (soundName.Equals(SoundyManagerConstant.NoSound))
                return null;

            SoundGroupData soundGroupData = Database.GetAudioData(databaseName, soundName);

            if (soundGroupData == null)
                return null;

            return soundGroupData.Play(position, Database.GetSoundDatabase(databaseName).OutputAudioMixerGroup);
        }

        public static SoundyController Play(AudioClip audioClip, Vector3 position)
        {
            if (s_initialized == false)
                s_instance = Instance;

            return Play(audioClip, null, position);
        }

        public static SoundyController Play(string databaseName, string soundName, Transform followTarget)
        {
            if (s_initialized == false)
                s_instance = Instance;

            if (Database == null)
                return null;

            if (soundName.Equals(SoundyManagerConstant.NoSound))
                return null;

            SoundGroupData soundGroupData = Database.GetAudioData(databaseName, soundName);

            if (soundGroupData == null)
                return null;

            return soundGroupData.Play(followTarget, Database.GetSoundDatabase(databaseName).OutputAudioMixerGroup);
        }

        public static SoundyController Play(AudioClip audioClip, Transform followTarget)
        {
            if (s_initialized == false)
                s_instance = Instance;

            return Play(audioClip, null, followTarget);
        }

        public static SoundyController Play(string databaseName, string soundName)
        {
            if (s_initialized == false)
                s_instance = Instance;

            if (Database == null)
                return null;

            if (soundName.Equals(SoundyManagerConstant.NoSound))
                return null;

            if (string.IsNullOrEmpty(databaseName) || string.IsNullOrEmpty(databaseName.Trim()))
                return null;

            if (string.IsNullOrEmpty(soundName) || string.IsNullOrEmpty(soundName.Trim()))
                return null;

            SoundDatabase soundDatabase = Database.GetSoundDatabase(databaseName);

            if (soundDatabase == null)
                return null;

            SoundGroupData soundGroupData = soundDatabase.GetData(soundName);

            if (soundGroupData == null)
                return null;

            return soundGroupData.Play(Pooler.transform, soundDatabase.OutputAudioMixerGroup);
        }

        public static SoundyController Play(AudioClip audioClip)
        {
            if (s_initialized == false)
                s_instance = Instance;

            return Play(audioClip, null, Pooler.transform);
        }

        public static SoundyController Play(
            AudioClip audioClip,
            AudioMixerGroup outputAudioMixerGroup,
            Vector3 position,
            float volume = 1,
            float pitch = 1,
            bool loop = false,
            float spatialBlend = 1)
        {
            if (s_initialized == false)
                s_instance = Instance;

            if (audioClip == null)
                return null;

            SoundyController controller = SoundyPooler.GetControllerFromPool();
            controller.SetSourceProperties(audioClip, volume, pitch, loop, spatialBlend);
            controller.SetOutputAudioMixerGroup(outputAudioMixerGroup);
            controller.SetPosition(position);
            controller.gameObject.name = "[AudioClip]-(" + audioClip.name + ")";
            controller.Name = audioClip.name;
            controller.Play();

            return controller;
        }

        public static SoundyController Play(
            AudioClip audioClip,
            AudioMixerGroup outputAudioMixerGroup,
            Transform followTarget = null,
            float volume = 1,
            float pitch = 1,
            bool loop = false,
            float spatialBlend = 1)
        {
            if (s_initialized == false)
                s_instance = Instance;

            if (audioClip == null)
                return null;

            SoundyController controller = SoundyPooler.GetControllerFromPool();
            controller.SetSourceProperties(audioClip, volume, pitch, loop, spatialBlend);
            controller.SetOutputAudioMixerGroup(outputAudioMixerGroup);

            if (followTarget == null)
            {
                spatialBlend = 0;
                controller.SetFollowTarget(Pooler.transform);
            }
            else
            {
                controller.SetFollowTarget(followTarget);
            }

            controller.gameObject.name = "[AudioClip]-(" + audioClip.name + ")";
            controller.Name = audioClip.name;
            controller.Play();

            return controller;
        }

        public static SoundyController Play(SoundyData data)
        {
            if (data == null)
                return null;

            if (s_initialized == false)
                s_instance = Instance;

            switch (data.SoundSource)
            {
                case SoundSource.Soundy:
                    return Play(data.DatabaseName, data.SoundName);
                case SoundSource.AudioClip:
                    return Play(data.AudioClip, data.OutputAudioMixerGroup);
                case SoundSource.MasterAudio:
#if dUI_MasterAudio
                    DarkTonic.MasterAudio.MasterAudio.PlaySound(data.SoundName);
#endif
                    break;
            }

            return null;
        }

        public static SoundyController Play(SoundyData data, bool isSound)
        {
            SoundyController controller = Play(data);
            controller.AudioSource.volume = isSound ? s_soundVolume : s_musicVolume;
            controller.AudioSource.mute = isSound ? s_isSoundVolumeMuted : s_isMusicVolumeMuted;
            
            return controller;
        }

        public static void Stop(string databaseName, string soundName) =>
            SoundyController.Stop(databaseName, soundName);

        public static void StopAllControllers() =>
            SoundyController.StopAll();

        public static void StopAllSounds()
        {
            StopAllControllers();
#if dUI_MasterAudio
            DarkTonic.MasterAudio.MasterAudio.StopEverything();
#endif
        }

        public static void UnmuteAllControllers() =>
            SoundyController.UnmuteAll();

        public static void UnmuteAllSounds()
        {
            UnmuteAllControllers();
#if dUI_MasterAudio
            DarkTonic.MasterAudio.MasterAudio.UnmuteEverything();
#endif
        }

        public static void UnpauseAllControllers() =>
            SoundyController.UnpauseAll();

        public static void UnpauseAllSounds()
        {
            UnpauseAllControllers();
#if dUI_MasterAudio
            DarkTonic.MasterAudio.MasterAudio.UnpauseEverything();
#endif
        }
    }
}