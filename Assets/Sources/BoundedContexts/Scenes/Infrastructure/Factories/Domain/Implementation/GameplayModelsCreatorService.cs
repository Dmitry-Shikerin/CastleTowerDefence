﻿using System;
using System.Collections.Generic;
using System.Linq;
using Sources.BoundedContexts.Bunkers.Domain;
using Sources.BoundedContexts.CharacterSpawnAbilities.Domain;
using Sources.BoundedContexts.EnemySpawners.Domain.Configs;
using Sources.BoundedContexts.EnemySpawners.Domain.Data;
using Sources.BoundedContexts.EnemySpawners.Domain.Models;
using Sources.BoundedContexts.FlamethrowerAbilities.Domain.Models;
using Sources.BoundedContexts.HealthBoosters.Domain;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.BoundedContexts.KillEnemyCounters.Domain.Models.Implementation;
using Sources.BoundedContexts.NukeAbilities.Domain.Models;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using Sources.BoundedContexts.Prefabs;
using Sources.BoundedContexts.Scenes.Domain;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Domain.Interfaces;
using Sources.BoundedContexts.Tutorials.Domain;
using Sources.BoundedContexts.Tutorials.Domain.Models;
using Sources.BoundedContexts.Upgrades.Domain.Configs;
using Sources.BoundedContexts.Upgrades.Domain.Data;
using Sources.BoundedContexts.Upgrades.Domain.Models;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;
using Sources.Frameworks.GameServices.Volumes.Domain.Models.Implementation;
using Sources.Frameworks.MyGameCreator.Achivements.Domain.Models;
using Sources.Frameworks.MyGameCreator.Achivements.Infrastructure.Services.Interfaces;
using UnityEngine;

namespace Sources.BoundedContexts.Scenes.Infrastructure.Factories.Domain.Implementation
{
    public class GameplayModelsCreatorService : IGameplayModelsLoaderService
    {
        private readonly IEntityRepository _entityRepository;
        private readonly ILoadService _loadService;
        private readonly UpgradeConfigContainer _upgradeConfigContainer;

        public GameplayModelsCreatorService(
            IAchievementService achievementService,
            IEntityRepository entityRepository,
            ILoadService loadService,
            UpgradeConfigContainer upgradeConfigContainer)
        {
            _entityRepository = entityRepository ?? 
                                throw new ArgumentNullException(nameof(entityRepository));
            _loadService = loadService ?? throw new ArgumentNullException(nameof(loadService));
            _upgradeConfigContainer = upgradeConfigContainer ?? 
                                      throw new ArgumentNullException(nameof(upgradeConfigContainer));
        }

        public GameplayModel Load()
        {
            //Upgrades
            Upgrade characterHealthUpgrade = CreateUpgrade(ModelId.HealthUpgrade);
            Upgrade characterAttackUpgrade = CreateUpgrade(ModelId.AttackUpgrade);
            Upgrade nukeAbilityUpgrade = CreateUpgrade(ModelId.NukeUpgrade);
            Upgrade flamethrowerAbilityUpgrade = CreateUpgrade(ModelId.FlamethrowerUpgrade);
            
            //Bunker
            Bunker bunker = new Bunker(15, ModelId.Bunker);
            _entityRepository.Add(bunker);
            
            //Enemies
            EnemySpawner enemySpawner = CreateEnemySpawner();
            
            KillEnemyCounter killEnemyCounter = new KillEnemyCounter()
            {
                Id = ModelId.KillEnemyCounter,
                KillZombies = 0,
            };
            _entityRepository.Add(killEnemyCounter);
            
            //Characters
            CharacterSpawnAbility characterSpawnAbility = new CharacterSpawnAbility(ModelId.SpawnAbility);
            _entityRepository.Add(characterSpawnAbility);
            
            //Abilities
            NukeAbility nukeAbility = new NukeAbility(nukeAbilityUpgrade, ModelId.NukeAbility);
            _entityRepository.Add(nukeAbility);
            
            FlamethrowerAbility flamethrowerAbility = new FlamethrowerAbility(
                flamethrowerAbilityUpgrade, ModelId.FlamethrowerAbility);
            _entityRepository.Add(flamethrowerAbility);
            
            //PlayerWallet
            PlayerWallet playerWallet = new PlayerWallet()
            {
                Coins = 15,
                Id = ModelId.PlayerWallet,
            };
            _entityRepository.Add(playerWallet);
            
            //Volume
            Volume musicVolume = LoadVolume(ModelId.MusicVolume);
            Volume soundsVolume = LoadVolume(ModelId.SoundsVolume);
            
            //Achievements
            List<Achievement> achievements = LoadAchievements();
            
            //HealthBooster
            HealthBooster healthBooster = new HealthBooster()
            {
                Id = ModelId.HealthBooster,
            };
            healthBooster.Amount++;
            _entityRepository.Add(healthBooster);
            
            //Tutorial
            Tutorial tutorial = CreateTutorial(ModelId.Tutorial);
            
            return new GameplayModel(
                characterHealthUpgrade,
                characterAttackUpgrade,
                nukeAbilityUpgrade,
                flamethrowerAbilityUpgrade,
                bunker,
                enemySpawner,
                characterSpawnAbility,
                nukeAbility,
                flamethrowerAbility,
                killEnemyCounter,
                playerWallet,
                musicVolume,
                soundsVolume,
                achievements,
                healthBooster,
                tutorial);
        }

        private Volume LoadVolume(string key)
        {
            if (_loadService.HasKey(key))
                return _loadService.Load<Volume>(key);

            Volume volume = new Volume(key);
            _entityRepository.Add(volume);
            
            return volume;
        }

        private Upgrade CreateUpgrade(string id)
        {
            UpgradeConfig config = _upgradeConfigContainer.UpgradeConfigs
                .First(config => config.Id == id);
            
            List<RuntimeUpgradeLevel> levels = new List<RuntimeUpgradeLevel>();
            config.Levels.ForEach(level => levels.Add(new RuntimeUpgradeLevel(level)));
            Upgrade upgrade = new Upgrade()
            {
                Levels = levels,
                Id = config.Id,
            };
            _entityRepository.Add(upgrade);

            return upgrade;
        }
        
        private List<Achievement> LoadAchievements()
        {
            List<Achievement> achievements = new List<Achievement>();

            if (_loadService.HasKey(ModelId.FirstUpgradeAchievement))
            {
                foreach (string id in ModelId.AchievementModels)
                {
                    Achievement achievement = _loadService.Load<Achievement>(id);
                    achievements.Add(achievement);
                }
                
                return achievements;
            }
            
            foreach (string id in ModelId.AchievementModels)
            {
                Achievement achievement = new Achievement(id);
                _entityRepository.Add(achievement);
                achievements.Add(achievement);
            }
            
            _loadService.Save(ModelId.AchievementModels);
            
            return achievements;
        }

        private Tutorial CreateTutorial(string key)
        {
            if (_loadService.HasKey(key))
                return _loadService.Load<Tutorial>(key);

            Tutorial tutorial = new Tutorial();
            _entityRepository.Add(tutorial);
            
            return tutorial;
        }

        private EnemySpawner CreateEnemySpawner()
        {
                        EnemySpawnerConfig enemySpawnerConfig = 
                Resources.Load<EnemySpawnerConfig>(
                    PrefabPath.EnemySpawnerConfigContainer);
            List<RuntimeEnemySpawnerWave> waves = new List<RuntimeEnemySpawnerWave>();
            foreach (EnemySpawnerWave wave in enemySpawnerConfig.Waves)
                waves.Add(new RuntimeEnemySpawnerWave()
                {
                    WaveId = wave.WaveId,
                    SpawnDelay = wave.SpawnDelay,
                    EnemyCount = wave.EnemyCount,
                    BossesCount = wave.BossesCount,
                    KamikazeEnemyCount = wave.KamikazeEnemyCount,
                    MoneyPerResilenceCharacters = wave.MoneyPerResilenceCharacters,
                });
            RuntimeEnemySpawnerConfig runtimeEnemySpawnerConfig = new RuntimeEnemySpawnerConfig()
            {
                StartEnemyAttackPower = enemySpawnerConfig.StartEnemyAttackPower,
                AddedEnemyAttackPower = enemySpawnerConfig.AddedEnemyAttackPower,
                StartEnemyHealth = enemySpawnerConfig.StartEnemyHealth,
                AddedEnemyHealth = enemySpawnerConfig.AddedEnemyHealth,
                StartBossAttackPower = enemySpawnerConfig.StartBossAttackPower,
                AddedBossAttackPower = enemySpawnerConfig.AddedBossAttackPower,
                StartBossMassAttackPower = enemySpawnerConfig.StartBossMassAttackPower,
                AddedBossMassAttackPower = enemySpawnerConfig.AddedBossMassAttackPower,
                StartBossHealth = enemySpawnerConfig.StartBossHealth,
                AddedBossHealth = enemySpawnerConfig.AddedBossHealth,
                StartKamikazeMassAttackPower = enemySpawnerConfig.StartKamikazeMassAttackPower,
                AddedKamikazeMassAttackPower = enemySpawnerConfig.AddedKamikazeMassAttackPower,
                StartKamikazeAttackPower = enemySpawnerConfig.StartKamikazeAttackPower,
                AddedKamikazeAttackPower = enemySpawnerConfig.AddedKamikazeAttackPower,
                StartKamikazeHealth = enemySpawnerConfig.StartKamikazeHealth,
                AddedKamikazeHealth = enemySpawnerConfig.AddedKamikazeHealth,
                Waves = waves,
            };
            EnemySpawner enemySpawner = new EnemySpawner()
            {
                Config = runtimeEnemySpawnerConfig,
                Id = ModelId.EnemySpawner
            };
            _entityRepository.Add(enemySpawner);
            
            return enemySpawner;
        }
    }
}