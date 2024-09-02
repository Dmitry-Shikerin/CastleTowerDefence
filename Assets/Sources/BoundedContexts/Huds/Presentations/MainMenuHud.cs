using System.Collections.Generic;
using Doozy.Runtime.UIManager.Components;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.HealthBoosters.Views;
using Sources.Frameworks.GameServices.DailyRewards.Presentation;
using Sources.Frameworks.GameServices.Volumes.Presentations;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.UI.Huds;
using Sources.Frameworks.MyGameCreator.Achievements.Presentation;
using Sources.Frameworks.UiFramework.Views.Presentations.Implementation;
using Sources.Frameworks.YandexSdkFramework.Leaderboards.Presentations.Implementation.Views;
using UnityEngine;

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

        [FoldoutGroup("Achievements")]
        [Required] [SerializeField] private List<AchievementView> _achievementViews;
        [FoldoutGroup("Achievements")]
        [Required] [SerializeField] private AchievementView _emptyAchievementView;

        [FoldoutGroup("LoadGame")]
        [Required] [SerializeField] private UIButton _loadGameButton;
        
        [FoldoutGroup("HealthBooster")]
        [Required] [SerializeField] private MainMenuHealthBoosterView _healthBoosterView;
        
        public IReadOnlyList<LeaderBoardElementView> LeaderBoardElementViews => _leaderBoardElementViews;
        public IReadOnlyList<AchievementView> AchievementViews => _achievementViews;
        public AchievementView EmptyAchievementView => _emptyAchievementView;
        public VolumeView MusicVolumeView => _musicVolumeView;
        public VolumeView SoundVolumeView => _soundVolumeView;
        public UiCollector UiCollector => _uiCollector;
        public DailyRewardView DailyRewardView => _dailyRewardView;
        public UIButton LoadGameButton => _loadGameButton;
        public MainMenuHealthBoosterView HealthBoosterView => _healthBoosterView;
    }
}