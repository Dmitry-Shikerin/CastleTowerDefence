using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sources.Frameworks.GameServices.DailyRewards.Presentation;
using Sources.Frameworks.GameServices.Volumes.Presentations;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.UI.Huds;
using Sources.Frameworks.UiFramework.Views.Presentations.Implementation;
using Sources.Frameworks.YandexSdcFramework.Presentations.Views;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sources.BoundedContexts.Huds.Presentations
{
    public class MainMenuHud : MonoBehaviour, IHud
    {
        [FoldoutGroup("Leaderboard")]
        [SerializeField] private List<LeaderBoardElementView> _leaderBoardElementViews;
        [FoldoutGroup("Volumes")]
        [SerializeField] private VolumeView _musicVolumeView;
        [FoldoutGroup("Volumes")]
        [SerializeField] private VolumeView _soundVolumeView;
        [FoldoutGroup("Collectors")]
        [SerializeField] private UiCollector _uiCollector;
        [FoldoutGroup("Rewards")]
        [Required] [SerializeField] private DailyRewardView _dailyRewardView;
        
        public IReadOnlyList<LeaderBoardElementView> LeaderBoardElementViews => _leaderBoardElementViews;
        public VolumeView MusicVolumeView => _musicVolumeView;
        public VolumeView SoundVolumeView => _soundVolumeView;
        public UiCollector UiCollector => _uiCollector;
        public DailyRewardView DailyRewardView => _dailyRewardView;
    }
}