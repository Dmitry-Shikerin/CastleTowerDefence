using System;
using System.Collections.Generic;
using Sources.BoundedContexts.Bunkers.Domain;
using Sources.BoundedContexts.CharacterSpawnAbilities.Domain;
using Sources.BoundedContexts.EnemySpawners.Domain.Models;
using Sources.BoundedContexts.FlamethrowerAbilities.Domain.Models;
using Sources.BoundedContexts.HealthBoosters.Domain;
using Sources.BoundedContexts.KillEnemyCounters.Domain.Models.Implementation;
using Sources.BoundedContexts.NukeAbilities.Domain.Models;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using Sources.BoundedContexts.Scenes.Domain;
using Sources.BoundedContexts.Tutorials.Domain.Models;
using Sources.BoundedContexts.Upgrades.Domain.Models;
using Sources.Frameworks.GameServices.Loads.Domain.Constant;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;
using Sources.Frameworks.GameServices.Scenes.Infrastructure.Factories.Domain.Interfaces;
using Sources.Frameworks.GameServices.Volumes.Domain.Models.Implementation;
using Sources.Frameworks.MyGameCreator.Achievements.Domain.Models;

namespace Sources.BoundedContexts.Scenes.Infrastructure.Factories.Domain.Implementation
{
    public class GameplayModelsLoaderService : IGameplayModelsLoaderService
    {
        private readonly IEntityRepository _entityRepository;
        private readonly ILoadService _loadService;

        public GameplayModelsLoaderService(
            IEntityRepository entityRepository,
            ILoadService loadService)
        {
            _entityRepository = entityRepository ?? throw new ArgumentNullException(nameof(entityRepository));
            _loadService = loadService ?? throw new ArgumentNullException(nameof(loadService));
        }

        public GameplayModel Load()
        {
            //Upgrades
            Upgrade characterHealthUpgrade = _loadService.Load<Upgrade>(ModelId.HealthUpgrade);
            Upgrade characterAttackUpgrade = _loadService.Load<Upgrade>(ModelId.AttackUpgrade);
            Upgrade nukeAbilityUpgrade = _loadService.Load<Upgrade>(ModelId.NukeUpgrade);
            Upgrade flamethrowerAbilityUpgrade = _loadService.Load<Upgrade>(ModelId.FlamethrowerUpgrade);
            
            //Bunker
            Bunker bunker = _loadService.Load<Bunker>(ModelId.Bunker);
            
            //Enemies
            EnemySpawner enemySpawner = _loadService.Load<EnemySpawner>(ModelId.EnemySpawner);
            KillEnemyCounter killEnemyCounter = _loadService.Load<KillEnemyCounter>(ModelId.KillEnemyCounter);
            
            //Characters
            CharacterSpawnAbility characterSpawnAbility = new CharacterSpawnAbility()
            {
                Id = ModelId.SpawnAbility,
            };
            _entityRepository.Add(characterSpawnAbility);
            
            //Abilities
            NukeAbility nukeAbility = new NukeAbility(nukeAbilityUpgrade, ModelId.NukeAbility);
            _entityRepository.Add(nukeAbility);
            
            FlamethrowerAbility flamethrowerAbility = new FlamethrowerAbility(
                flamethrowerAbilityUpgrade, ModelId.FlamethrowerAbility);
            _entityRepository.Add(flamethrowerAbility);
            
            //PlayerWallet
            PlayerWallet playerWallet = _loadService.Load<PlayerWallet>(ModelId.PlayerWallet);
            
            //Volumes
            Volume musicVolume = _loadService.Load<Volume>(ModelId.MusicVolume);
            Volume soundsVolume = _loadService.Load<Volume>(ModelId.SoundsVolume);
            
            //Achievements
            List<Achievement> achievements = new List<Achievement>();
            
            foreach (string id in ModelId.GetIds<Achievement>())
            {
                Achievement achievement = _loadService.Load<Achievement>(id);
                achievements.Add(achievement);
            }
            
            //HealthBooster
            HealthBooster healthBooster = _loadService.Load<HealthBooster>(ModelId.HealthBooster);
            
            //Tutorial
            Tutorial tutorial = _loadService.Load<Tutorial>(ModelId.Tutorial);
            
            // Debug.Log($"Load models");
            //
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
    }
}