using System;
using System.Linq;
using Sources.BoundedContexts.Bunkers.Domain;
using Sources.BoundedContexts.CharacterSpawnAbilities.Domain;
using Sources.BoundedContexts.EnemySpawners.Domain.Configs;
using Sources.BoundedContexts.EnemySpawners.Domain.Models;
using Sources.BoundedContexts.FlamethrowerAbilities.Domain.Models;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.BoundedContexts.KillEnemyCounters.Domain.Models.Implementation;
using Sources.BoundedContexts.NukeAbilities.Domain.Models;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using Sources.BoundedContexts.Prefabs;
using Sources.BoundedContexts.Scenes.Domain;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Domain.Interfaces;
using Sources.BoundedContexts.Upgrades.Domain.Configs;
using Sources.BoundedContexts.Upgrades.Domain.Models;
using Sources.Frameworks.GameServices.Volumes.Domain.Models.Implementation;
using Sources.InfrastructureInterfaces.Services.Repositories;
using UnityEngine;

namespace Sources.BoundedContexts.Scenes.Infrastructure.Factories.Domain.Implementation
{
    public class GameplayModelsCreatorService : IGameplayModelsLoaderService
    {
        private readonly IEntityRepository _entityRepository;
        private readonly UpgradeConfigContainer _upgradeConfigContainer;

        public GameplayModelsCreatorService(
            IEntityRepository entityRepository,
            UpgradeConfigContainer upgradeConfigContainer)
        {
            _entityRepository = entityRepository ?? 
                                throw new ArgumentNullException(nameof(entityRepository));
            _upgradeConfigContainer = upgradeConfigContainer ?? 
                                      throw new ArgumentNullException(nameof(upgradeConfigContainer));
        }

        public GameplayModel Load()
        {
            //Upgrades
            Upgrade characterHealthUpgrade = new Upgrade(
                _upgradeConfigContainer.UpgradeConfigs.First(config => config.Id == ModelId.HealthUpgrade));
            _entityRepository.Add(characterHealthUpgrade);
            
            Upgrade characterAttackUpgrade = new Upgrade(
                _upgradeConfigContainer.UpgradeConfigs.First(config => config.Id == ModelId.AttackUpgrade));
            _entityRepository.Add(characterAttackUpgrade);
            
            Upgrade nukeAbilityUpgrade = new Upgrade(
                _upgradeConfigContainer.UpgradeConfigs.First(config => config.Id == ModelId.NukeUpgrade));
            _entityRepository.Add(nukeAbilityUpgrade);
            
            Upgrade flamethrowerAbilityUpgrade = new Upgrade(
                _upgradeConfigContainer.UpgradeConfigs.First(config => config.Id == ModelId.FlamethrowerUpgrade));
            _entityRepository.Add(flamethrowerAbilityUpgrade);
            
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
            Volume volume = new Volume(ModelId.Volume);
            _entityRepository.Add(volume);
            
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
                volume);
        }
    }
}