using System;
using System.Collections.Generic;
using System.Linq;
using Sources.BoundedContexts.Bunkers.Domain;
using Sources.BoundedContexts.CharacterSpawnAbilities.Domain;
using Sources.BoundedContexts.EnemySpawners.Domain.Configs;
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
using Sources.BoundedContexts.Upgrades.Domain.Configs;
using Sources.BoundedContexts.Upgrades.Domain.Models;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces;
using Sources.Frameworks.GameServices.Volumes.Domain.Models.Implementation;
using Sources.Frameworks.MyGameCreator.Achivements.Domain.Models;
using Sources.Frameworks.MyGameCreator.Achivements.Infrastructure.Services.Interfaces;
using Sources.InfrastructureInterfaces.Services.Repositories;
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
            EnemySpawnerConfig enemySpawnerConfig = 
                Resources.Load<EnemySpawnerConfig>(
                    PrefabPath.EnemySpawnerConfigContainer);
            EnemySpawner enemySpawner = new EnemySpawner(enemySpawnerConfig, ModelId.EnemySpawner);
            _entityRepository.Add(enemySpawner);
            
            KillEnemyCounter killEnemyCounter = new KillEnemyCounter(ModelId.KillEnemyCounter, 0);
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
            PlayerWallet playerWallet = new PlayerWallet(50, ModelId.PlayerWallet);
            _entityRepository.Add(playerWallet);
            
            //Volume
            Volume musicVolume = LoadVolume(ModelId.MusicVolume);
            Volume soundsVolume = LoadVolume(ModelId.SoundsVolume);
            
            //Achievements
            List<Achievement> achievements = LoadAchievements();
            
            //HealthBooster
            HealthBooster healthBooster = new HealthBooster(ModelId.HealthBooster);
            healthBooster.Amount++;
            _entityRepository.Add(healthBooster);
            
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
                healthBooster);
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
            Upgrade upgrade = new Upgrade(config);
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
    }
}