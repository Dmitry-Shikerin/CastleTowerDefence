using System;
using JetBrains.Annotations;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.BoundedContexts.Scenes.Domain;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces;
using Sources.Frameworks.GameServices.Volumes.Domain.Models.Implementation;
using Sources.InfrastructureInterfaces.Services.Repositories;
using UnityEngine;

namespace Sources.BoundedContexts.Scenes.Infrastructure.Factories.Domain.Implementation
{
    public class MainMenuModelsCreatorService
    {
        private readonly ILoadService _loadService;
        private readonly IEntityRepository _entityRepository;

        public MainMenuModelsCreatorService(
            ILoadService loadService,
            IEntityRepository entityRepository)
        {
            _loadService = loadService ?? throw new ArgumentNullException(nameof(loadService));
            _entityRepository = entityRepository ?? throw new ArgumentNullException(nameof(entityRepository));
        }

        public MainMenuModel Load()
        {
            Volume musicVolume = new Volume(ModelId.MusicVolume);
            _entityRepository.Add(musicVolume);
            Volume soundsVolume = new Volume(ModelId.SoundsVolume);
            _entityRepository.Add(soundsVolume);
            
            _loadService.Save(ModelId.MainMenuModels);
            Debug.Log($"Create models");
            
            return new MainMenuModel(
                musicVolume, 
                soundsVolume);
        }
    }
}