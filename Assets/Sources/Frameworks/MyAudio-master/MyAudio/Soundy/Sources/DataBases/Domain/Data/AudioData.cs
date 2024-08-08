using System;
using MyAudios.Soundy.Sources.DataBases.Domain.Constants;
using UnityEngine;

namespace MyAudios.Soundy.Sources.DataBases.Domain.Data
{
    /// <summary> Audio info for any referenced AudioClip in the Soundy system </summary>
    [Serializable]
    public class AudioData
    {
        /// <summary> Direct reference to an AudioClip </summary>
        public AudioClip AudioClip;

        /// <summary> (Not Implemented) Weight of this AudioClip in the SoundGroupData </summary>
        [Range(AudioDataConst.MinWeight, AudioDataConst.MaxWeight)]
        public float Weight = AudioDataConst.DefaultWeight;

        public bool IsPlaying;
        
        /// <summary> Creates a new instance for this class </summary>
        public AudioData() =>
            Reset();

        /// <summary> Creates a new instance for this class and sets the given AudioClip reference </summary>
        /// <param name="audioClip"> AudioClip reference </param>
        public AudioData(AudioClip audioClip)
        {
            Reset();
            AudioClip = audioClip;
        }

        /// <summary> Creates a new instance for this class and sets the given AudioClip reference with the given weight </summary>
        /// <param name="audioClip"> AudioClip reference </param>
        /// <param name="weight"> (Not Implemented) AudioClip weight </param>
        public AudioData(AudioClip audioClip, float weight)
        {
            Reset();
            AudioClip = audioClip;
            Weight = weight;
        }

        /// <summary> Resets this instance to the default values </summary>
        public void Reset()
        {
            AudioClip = null;
            Weight = 1;
        }
    }
}