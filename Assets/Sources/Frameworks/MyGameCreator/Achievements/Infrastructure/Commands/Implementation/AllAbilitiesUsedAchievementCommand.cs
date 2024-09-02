using System;
using Sources.BoundedContexts.CharacterSpawnAbilities.Domain;
using Sources.BoundedContexts.FlamethrowerAbilities.Domain.Models;
using Sources.BoundedContexts.NukeAbilities.Domain.Models;
using Sources.Frameworks.GameServices.Loads.Domain.Constant;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces;
using Sources.Frameworks.GameServices.Prefabs.Interfaces;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;
using Sources.Frameworks.MyGameCreator.Achievements.Domain.Models;
using Sources.Frameworks.MyGameCreator.Achievements.Infrastructure.Commands.Implementation.Base;
using Sources.Frameworks.MyGameCreator.Achievements.Presentation;
using Zenject;

namespace Sources.Frameworks.MyGameCreator.Achievements.Infrastructure.Commands.Implementation
{
    public class AllAbilitiesUsedAchievementCommand : AchievementCommandBase
    {
        private readonly IEntityRepository _entityRepository;

        private NukeAbility _nukeAbility;
        private FlamethrowerAbility _flamethrowerAbility;
        private CharacterSpawnAbility _characterSpawnAbility;
        private Achievement _achievement;
        private AchievementView _achievementView;

        public AllAbilitiesUsedAchievementCommand(
            IEntityRepository entityRepository,
            IAssetCollector assetCollector,
            ILoadService loadService,
            AchievementView achievementView,
            DiContainer container) 
            : base(
                achievementView, 
                assetCollector,
                loadService,
                container)
        {
            _entityRepository = entityRepository ?? 
                                throw new ArgumentNullException(nameof(entityRepository));
        }

        public override void Initialize()
        {
            base.Initialize();
            
            _nukeAbility = _entityRepository
                .Get<NukeAbility>(ModelId.NukeAbility);
            _flamethrowerAbility = _entityRepository
                .Get<FlamethrowerAbility>(ModelId.FlamethrowerAbility);
            _characterSpawnAbility = _entityRepository
                .Get<CharacterSpawnAbility>(ModelId.SpawnAbility);
            _achievement = _entityRepository
                .Get<Achievement>(ModelId.AllAbilitiesUsedAchievementCommand);

            _nukeAbility.AbilityApplied += OnCompleted;
            _flamethrowerAbility.AbilityApplied += OnCompleted;
            _characterSpawnAbility.AbilityApplied += OnCompleted;
        }

        private void OnCompleted()
        {
            if (_achievement.IsCompleted)
                return;

            if (_nukeAbility.IsApplied == false)
                return;
            
            if (_flamethrowerAbility.IsApplied == false)
                return;
            
            if (_characterSpawnAbility.IsApplied == false)
                return;

            Execute(_achievement);
        }

        public override void Destroy()
        {
            _nukeAbility.AbilityApplied -= OnCompleted;
            _flamethrowerAbility.AbilityApplied -= OnCompleted;
            _characterSpawnAbility.AbilityApplied -= OnCompleted;
        }
    }
}