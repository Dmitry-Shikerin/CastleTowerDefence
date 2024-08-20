using System;
using System.Collections.Generic;
using Doozy.Runtime.UIManager;
using Sources.BoundedContexts.Bunkers.Domain;
using Sources.BoundedContexts.EnemySpawners.Domain.Data;
using Sources.BoundedContexts.EnemySpawners.Domain.Models;
using Sources.BoundedContexts.FlamethrowerAbilities.Domain.Models;
using Sources.BoundedContexts.KillEnemyCounters.Domain.Models.Implementation;
using Sources.BoundedContexts.NukeAbilities.Domain.Models;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using Sources.BoundedContexts.Tutorials.Domain;
using Sources.BoundedContexts.Upgrades.Domain.Data;
using Sources.BoundedContexts.Upgrades.Domain.Models;
using Sources.Domain.Models.Data;
using Sources.Frameworks.Domain.Implementation.Data;
using Sources.Frameworks.GameServices.DailyRewards.Domain;
using Sources.Frameworks.GameServices.Volumes.Domain.Models.Implementation;
using Sources.Frameworks.MyGameCreator.Achivements.Domain.Models;

namespace Sources.BoundedContexts.Ids.Domain.Constant
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
        //public const string FirstHealthRemovedAchievement = "FirstHealthRemovedAchievement";
        public const string FirstUpgradeAchievement = "FirstUpgradeAchievement";
        public const string FirstHealthBoosterUsageAchievement = "FirstHealthBoosterUsageAchievement";
        public const string FirstWaveCompletedAchievement = "FirstWaveCompletedAchievement";
        public const string ScullsDiggerAchievement = "ScullsDiggerAchievement";
        public const string MaxUpgradeAchievement = "MaxUpgradeAchievement";


        public static IReadOnlyList<string> AchievementModels { get; } = new List<string>()
        {
            FirstEnemyKillAchievement,
            //FirstHealthRemovedAchievement,
            FirstUpgradeAchievement,
            FirstHealthBoosterUsageAchievement,
            FirstWaveCompletedAchievement,
            ScullsDiggerAchievement,
            MaxUpgradeAchievement,
        };

        public static IReadOnlyList<string> MainMenuModels { get; } = new List<string>()
        {
            SoundsVolume,
            MusicVolume,
            DailyReward,
        };

        public static IReadOnlyList<string> DeletedModelsIds { get; } = new List<string>()
        {
            PlayerWallet,
            KillEnemyCounter,
        };

        public static IReadOnlyList<string> ModelsIds { get; } = new List<string>()
        {
            ScoreCounter,
            SavedLevel,
            GameData,
            Gameplay,
            PlayerWallet,
            KillEnemyCounter,
            Tutorial,
            SoundsVolume,
            MusicVolume
        };

        public static IReadOnlyDictionary<string, Type> Types { get; } = new Dictionary<string, Type>()
        {
            [HealthUpgrade] = typeof(Upgrade),
            [AttackUpgrade] = typeof(Upgrade),
            [NukeUpgrade] = typeof(Upgrade),
            [FlamethrowerUpgrade] = typeof(Upgrade),
            [PlayerWallet] = typeof(PlayerWallet),
            [Bunker] = typeof(Bunker),
            [EnemySpawner] = typeof(EnemySpawner),
            [NukeAbility] = typeof(NukeAbility),
            [FlamethrowerAbility] = typeof(FlamethrowerAbility),
            [KillEnemyCounter] = typeof(KillEnemyCounter),
            [MusicVolume] = typeof(Volume),
            [SoundsVolume] = typeof(Volume),
            [DailyReward] = typeof(DailyReward),
            [FirstEnemyKillAchievement] = typeof(Achievement),
            [FirstUpgradeAchievement] = typeof(Achievement),
            [FirstHealthBoosterUsageAchievement] = typeof(Achievement),
            [FirstWaveCompletedAchievement] = typeof(Achievement),
            [ScullsDiggerAchievement] = typeof(Achievement),
            [MaxUpgradeAchievement] = typeof(Achievement),
            [Tutorial] = typeof(Tutorial),
        };
    }
}