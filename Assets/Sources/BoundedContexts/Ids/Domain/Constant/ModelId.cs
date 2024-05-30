using System;
using System.Collections.Generic;
using Sources.BoundedContexts.EnemySpawners.Domain.Data;
using Sources.BoundedContexts.Upgrades.Domain.Data;
using Sources.Domain.Models.Data;
using Sources.Frameworks.Domain.Implementation.Data;

namespace Sources.BoundedContexts.Ids.Domain.Constant
{
    public static class ModelId
    {
        //gameModels
        public const string GameplayEnemySpawner = "GameplayEnemySpawner";
        public const string SawLauncherAbilityUpgrader = "SawLauncherAbilityUpgrader";
        public const string SawLauncherUpgrader = "SawLauncherUpgrader";
        public const string PlayerWallet = "PlayerWallet";
        public const string MiniGunAttackUpgrader = "MiniGunAttackUpgrader";
        public const string CharacterHealthUpgrader = "CharacterHealthUpgrader";
        public const string BearAttackUpgrader = "BearAttackUpgrader";
        public const string BearMassAttackUpgrader = "BearMassAttackUpgrader";
        public const string KillEnemyCounter = "KillEnemyCounter";

        //commonModels
        public const string ScoreCounter = "ScoreCounter";
        public const string MainMenu = "MainMenu";
        public const string SavedLevel = "SavedLevel";
        public const string Volume = "Volume";
        public const string GameData = "GameData";
        public const string Tutorial = "Tutorial";
        public const string Gameplay = "Gameplay";
        public const string Gameplay2 = "GamePlay2";
        public const string Gameplay3 = "GamePlay3";
        public const string Gameplay4 = "GamePlay4";

        public static IReadOnlyList<string> DeletedModelsIds { get; } = new List<string>()
        {
            GameplayEnemySpawner,
            SawLauncherAbilityUpgrader,
            SawLauncherUpgrader,
            PlayerWallet,
            MiniGunAttackUpgrader,
            CharacterHealthUpgrader,
            BearAttackUpgrader,
            BearMassAttackUpgrader,
            KillEnemyCounter,
        };
        
        //todo переделать
        public static IReadOnlyList<string> ModelsIds { get; } = new List<string>()
        {
            ScoreCounter,
            GameplayEnemySpawner,
            SavedLevel,
            GameData,
            Gameplay,
            Gameplay2,
            Gameplay3,
            Gameplay4,
            Volume,
            SawLauncherAbilityUpgrader,
            SawLauncherUpgrader,
            PlayerWallet,
            MiniGunAttackUpgrader,
            CharacterHealthUpgrader,
            BearAttackUpgrader,
            BearMassAttackUpgrader,
            KillEnemyCounter,
            Tutorial,
        };

        //TODO заменить такую запись на поиск из ассембли
        public static IReadOnlyDictionary<string, Type> DtoTypes { get; } = new Dictionary<string, Type>()
        {
            [ScoreCounter] = typeof(ScoreCounterDto),
            [GameplayEnemySpawner] = typeof(EnemySpawnerDto),
            [KillEnemyCounter] = typeof(KillEnemyCounterDto),
            [Tutorial] = typeof(TutorialDto),
            [GameData] = typeof(GameDataDto),
            [Volume] = typeof(VolumeDto),
            [Gameplay] = typeof(LevelDto),
            [Gameplay2] = typeof(LevelDto),
            [Gameplay3] = typeof(LevelDto),
            [Gameplay4] = typeof(LevelDto),
            [SawLauncherAbilityUpgrader] = typeof(UpgradeDto),
            [SawLauncherUpgrader] = typeof(UpgradeDto),
            [PlayerWallet] = typeof(PlayerWalletDto),
            [MiniGunAttackUpgrader] = typeof(UpgradeDto),
            [CharacterHealthUpgrader] = typeof(UpgradeDto),
            [BearAttackUpgrader] = typeof(UpgradeDto),
            [BearMassAttackUpgrader] = typeof(UpgradeDto),
        };
    }
}