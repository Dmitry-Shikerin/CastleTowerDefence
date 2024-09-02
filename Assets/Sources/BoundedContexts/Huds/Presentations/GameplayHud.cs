using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.Abilities.Presentation.Implementation;
using Sources.BoundedContexts.AdvertisingAfterWaves.Presentation;
using Sources.BoundedContexts.Bunkers.Presentation.Implementation;
using Sources.BoundedContexts.EnemySpawners.Presentation.Implementation;
using Sources.BoundedContexts.HealthBoosters.Views;
using Sources.BoundedContexts.PlayerWallets.Presentation.Implementation;
using Sources.BoundedContexts.Upgrades.Presentation.Implementation;
using Sources.Frameworks.GameServices.Volumes.Presentations;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.UI.Huds;
using Sources.Frameworks.MyGameCreator.Achievements.Presentation;
using Sources.Frameworks.UiFramework.Views.Presentations.Implementation;
using UnityEngine;

namespace Sources.BoundedContexts.Huds.Presentations
{
    public class GameplayHud : MonoBehaviour, IHud
    {
        [FoldoutGroup("UiFramework")]
        [Required] [SerializeField] private UiCollector _uiCollector;
        
        [FoldoutGroup("Abilities")]
        [Required] [SerializeField] private AbilityApplierView _nukeAbilityApplier;
        [FoldoutGroup("Abilities")]
        [Required] [SerializeField] private AbilityApplierView _spawnAbilityApplier;
        [FoldoutGroup("Abilities")]
        [Required] [SerializeField] private AbilityApplierView _flamethrowerAbilityApplier;

        [FoldoutGroup("UiFramework")] 
        [Required] [SerializeField] private EnemySpawnerUi _enemySpawnerUi;
        
        [FoldoutGroup("Bunker")] 
        [Required] [SerializeField] private BunkerUi _bunkerUi;
        
        [FoldoutGroup("Upgrades")]
        [Required] [SerializeField] private UpgradeView _characterAttackUpgradeView;
        [FoldoutGroup("Upgrades")]
        [Required] [SerializeField] private UpgradeView _characterHealthUpgradeView;
        [FoldoutGroup("Upgrades")]
        [Required] [SerializeField] private UpgradeView _nukeAbilityUpgradeVieew;
        [FoldoutGroup("Upgrades")]
        [Required] [SerializeField] private UpgradeView _flamethrowerAbilityUpgradeView;
        
        [FoldoutGroup("Wallet")]
        [Required] [SerializeField] private PlayerWalletView _playerWalletView;
        
        [FoldoutGroup("Volume")]
        [Required] [SerializeField] private VolumeView _musicVolumeView;
        [FoldoutGroup("Volume")]
        [Required] [SerializeField] private VolumeView _soundVolumeView;
        
        [FoldoutGroup("HealthBooster")]
        [Required] [SerializeField] private HealthBoosterView _healthBoosterView;

        [FoldoutGroup("Achievements")]
        [Required] [SerializeField] private AchievementView _popUpAchievementView;
        [FoldoutGroup("Achievements")]
        [Required] [SerializeField] private List<AchievementView> _achievementViews;
        
        [FoldoutGroup("Ad")]
        [Required] [SerializeField] private AdvertisingAfterWaveView _advertisingView;
        
        public UiCollector UiCollector => _uiCollector;

        public AbilityApplierView NukeAbilityApplier => _nukeAbilityApplier;
        public AbilityApplierView SpawnAbilityApplier => _spawnAbilityApplier;
        public AbilityApplierView FlamethrowerAbilityApplier => _flamethrowerAbilityApplier;
        
        public EnemySpawnerUi EnemySpawnerUi => _enemySpawnerUi;
        
        public BunkerUi BunkerUi => _bunkerUi;
        
        public UpgradeView CharacterHealthUpgradeView => _characterHealthUpgradeView;
        public UpgradeView CharacterAttackUpgradeView => _characterAttackUpgradeView;
        public UpgradeView NukeAbilityUpgradeView => _nukeAbilityUpgradeVieew;
        public UpgradeView FlamethrowerAbilityUpgradeView => _flamethrowerAbilityUpgradeView;
        
        public PlayerWalletView PlayerWalletView => _playerWalletView;
        public VolumeView MusicVolumeView => _musicVolumeView;
        public VolumeView SoundVolumeView => _soundVolumeView;
        public HealthBoosterView HealthBoosterView => _healthBoosterView;
        public AchievementView PopUpAchievementView => _popUpAchievementView;
        public IReadOnlyList<AchievementView> AchievementViews => _achievementViews;
        public AdvertisingAfterWaveView AdvertisingView => _advertisingView;
    }
}