using System;
using JetBrains.Annotations;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.BoundedContexts.Scenes.Domain;
using Sources.Frameworks.GameServices.Volumes.Domain.Models.Implementation;
using Sources.InfrastructureInterfaces.Services.Repositories;

namespace Sources.BoundedContexts.Scenes.Infrastructure.Factories.Domain.Implementation
{
    public class MainMenuModelsCreatorService
    {
        private readonly IEntityRepository _entityRepository;

        public MainMenuModelsCreatorService(
            IEntityRepository entityRepository)
        {
            _entityRepository = entityRepository ?? throw new ArgumentNullException(nameof(entityRepository));
        }

        public MainMenuModel Load()
        {
            Volume musicVolume = new Volume(ModelId.MusicVolume);
            _entityRepository.Add(musicVolume);
            Volume soundsVolume = new Volume(ModelId.SoundsVolume);
            _entityRepository.Add(soundsVolume);
            
            return new MainMenuModel(
                musicVolume, 
                soundsVolume);
        }
    }
}