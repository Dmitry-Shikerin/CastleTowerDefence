using Sources.Frameworks.UiFramework.AudioSources.Domain.Dictionaries;
using UnityEngine;

namespace Sources.Frameworks.UiFramework.AudioSources.Domain.Configs
{
    [CreateAssetMenu(fileName = "AudioClipContainer", menuName = "Configs/AudioClipContainer", order = 51)]
    public class AudioServiceDataBase : ScriptableObject
    {
        [SerializeField] private AudioClipDictionary _audioClips;
        [Space(10f)]
        [SerializeField] private AudioClipGroupDictionary _audioClipGroups;
        [SerializeField] private int _poolCount = 3;
         
        public AudioClipDictionary AudioClips => _audioClips;
        public AudioClipGroupDictionary AudioGroups => _audioClipGroups;
        public int PoolCount => _poolCount;
    }
}