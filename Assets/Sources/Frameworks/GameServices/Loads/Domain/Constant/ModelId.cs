using System.Collections.Generic;
using System.Linq;
using Sources.BoundedContexts.Bunkers.Domain;
using Sources.BoundedContexts.EnemySpawners.Domain.Models;
using Sources.BoundedContexts.FlamethrowerAbilities.Domain.Models;
using Sources.BoundedContexts.HealthBoosters.Domain;
using Sources.BoundedContexts.KillEnemyCounters.Domain.Models.Implementation;
using Sources.BoundedContexts.NukeAbilities.Domain.Models;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using Sources.BoundedContexts.Tutorials.Domain.Models;
using Sources.BoundedContexts.Upgrades.Domain.Models;
using Sources.Frameworks.Domain.Interfaces.Entities;
using Sources.Frameworks.GameServices.DailyRewards.Domain;
using Sources.Frameworks.GameServices.Volumes.Domain.Models.Implementation;
using Sources.Frameworks.MyGameCreator.Achievements.Domain.Models;

namespace Sources.Frameworks.GameServices.Loads.Domain.Constant
{
    public static class ModelId
    {
        //upgrades
        public const string HealthUpgrade = "HealthUpgrade";
        public const string AttackUpgrade = "AttackUpgrade";
        public const string NukeUpgrade = "NukeUpgrade";
        public const string FlamethrowerUpgrade = "FlamethrowerUpgrade";

        //gameModels
        public const string PlayerWallet = "PlayerWallet";
        public const string Bunker = "Bunker";
        public const string EnemySpawner = "EnemySpawner";
        public const string SpawnAbility = "SpawnAbility";
        public const string NukeAbility = "NukeAbility";
        public const string FlamethrowerAbility = "FlamethrowerAbility";
        public const string KillEnemyCounter = "KillEnemyCounter";

        //commonModels
        public const string DailyReward = "DailyReward";
        public const string HealthBooster = "HealthBooster";
        public const string ScoreCounter = "ScoreCounter";
        public const string MainMenu = "MainMenu";
        public const string SavedLevel = "SavedLevel";
        public const string MusicVolume = "MusicVolume";
        public const string SoundsVolume = "SoundVolume";
        public const string GameData = "GameData";
        public const string Tutorial = "Tutorial";
        public const string Gameplay = "Gameplay";
        
        //Achievements
        public const string FirstEnemyKillAchievement = "FirstEnemyKillAchievement";
        public const string FirstUpgradeAchievement = "FirstUpgradeAchievement";
        public const string FirstHealthBoosterUsageAchievement = "FirstHealthBoosterUsageAchievement";
        public const string FirstWaveCompletedAchievement = "FirstWaveCompletedAchievement";
        public const string ScullsDiggerAchievement = "ScullsDiggerAchievement";
        public const string MaxUpgradeAchievement = "MaxUpgradeAchievement";
        public const string FiftyWaveCompletedAchievement = "FiftyWaveCompletedAchievement";
        public const string AllAbilitiesUsedAchievementCommand = "AllAbilitiesUsedAchievementCommand";
        public const string CompleteGameWithOneHealthAchievementCommand = "CompleteGameWithOneHealthAchievementCommand";
        
        public static IReadOnlyDictionary<string, EntityData> ModelData { get; } = new Dictionary<string, EntityData>()
        {
             [HealthUpgrade] = new (HealthUpgrade, typeof(Upgrade), true),
             [AttackUpgrade] = new (AttackUpgrade, typeof(Upgrade), true),
             [NukeUpgrade] = new (NukeUpgrade, typeof(Upgrade), true),
             [FlamethrowerUpgrade] = new (FlamethrowerUpgrade, typeof(Upgrade), true),
             [PlayerWallet] = new (PlayerWallet, typeof(PlayerWallet), true),
             [Bunker] = new (Bunker, typeof(Bunker), true),
             [EnemySpawner] = new (EnemySpawner, typeof(EnemySpawner), true),
             [NukeAbility] = new (NukeAbility, typeof(NukeAbility), true),
             [FlamethrowerAbility] = new (FlamethrowerAbility, typeof(FlamethrowerAbility), true),
             [KillEnemyCounter] = new (KillEnemyCounter, typeof(KillEnemyCounter), true),
             [MusicVolume] = new (MusicVolume, typeof(Volume), false),
             [SoundsVolume] = new (SoundsVolume, typeof(Volume), false),
             [DailyReward] = new (DailyReward, typeof(DailyReward), false),
             [FirstEnemyKillAchievement] = new (FirstEnemyKillAchievement, typeof(Achievement), false),
             [FirstUpgradeAchievement] = new (FirstUpgradeAchievement, typeof(Achievement), false),
             [FirstHealthBoosterUsageAchievement] = new (FirstHealthBoosterUsageAchievement, typeof(Achievement), false),
             [FirstWaveCompletedAchievement] = new (FirstWaveCompletedAchievement, typeof(Achievement), false),
             [ScullsDiggerAchievement] = new (ScullsDiggerAchievement, typeof(Achievement), false),
             [MaxUpgradeAchievement] = new (MaxUpgradeAchievement, typeof(Achievement), false),
             [FiftyWaveCompletedAchievement] = new (FiftyWaveCompletedAchievement, typeof(Achievement), false),
             [AllAbilitiesUsedAchievementCommand] = new (AllAbilitiesUsedAchievementCommand, typeof(Achievement), false),
             [CompleteGameWithOneHealthAchievementCommand] = new (CompleteGameWithOneHealthAchievementCommand, typeof(Achievement), false),
             [Tutorial] = new (Tutorial, typeof(Tutorial), false),
             [HealthBooster] = new (HealthBooster, typeof(HealthBooster), false),
        };

        public static IReadOnlyList<string> GetIds<T>() 
            where T : IEntity
        {
            return ModelData.Values
                .Where(data => data.Type == typeof(T))
                .Select(data => data.ID)
                .ToList();
        }

        public static IReadOnlyList<string> GetDeleteIds()
        {
            return ModelData.Values
                .Where(data => data.IsDeleted)
                .Select(data => data.ID)
                .ToList();
        }
    }
}