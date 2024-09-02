using System;
using Sirenix.OdinInspector;
using Sources.Frameworks.GameServices.Volumes.Domain.Models.Implementation;
using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.Views.Constructors;
using Sources.Frameworks.UiFramework.AudioSources.Domain.Dictionaries;
using Sources.Frameworks.UiFramework.Core.Domain.Constants;
using UnityEngine;

namespace Sources.Frameworks.UiFramework.AudioSources.Domain.Configs
{
    [CreateAssetMenu(fileName = "AudioClipContainer", menuName = "Configs/AudioClipContainer", order = 51)]
    public class AudioServiceDataBase : ScriptableObject, IConstruct<Volume>
    {
        [DisplayAsString(false)] [HideLabel] 
        [SerializeField] private string _labele = UiConstant.AudioServiceDataBaseLabel;

        [BoxGroup("Settings")]
        [SerializeField] private int _poolCount = 3;
        [BoxGroup("Settings")] [Range(0, 1)] [OnValueChanged("ChangeVolume")]
        [SerializeField] private float _volume = 0.5f;

        [TabGroup("Clips")]
        [SerializeField] private AudioClipDictionary _audioClips;

        [TabGroup("Groups")] [Space(10f)]
        [SerializeField] private AudioClipGroupDictionary _audioClipGroups;

        private Volume _volumeModel;

        public AudioClipDictionary AudioClips => _audioClips;
        public AudioClipGroupDictionary AudioGroups => _audioClipGroups;
        public int PoolCount => _poolCount;
        public float Volume => _volume;

        public void Construct(Volume leaderBoardElementViews) =>
            _volumeModel = leaderBoardElementViews ?? throw new ArgumentNullException(nameof(leaderBoardElementViews));
        
        private void ChangeVolume()
        {
            if (_volumeModel == null)
                return;
            
            _volumeModel.VolumeValue = _volume;
        }
    }
}