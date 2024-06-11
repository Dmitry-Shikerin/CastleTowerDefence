using System;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Domain;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Interfaces;
using Sources.Frameworks.GameServices.Scenes.Services.Interfaces;
using Sources.InfrastructureInterfaces.Services.Repositories;

namespace Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Implementation
{
    public class LoadMainMenuSceneCommand : IButtonCommand
    {
        private readonly ISceneService _sceneService;
        private readonly IEntityRepository _entityRepository;

        public LoadMainMenuSceneCommand(
            ISceneService sceneService,
            IEntityRepository entityRepository)
        {
            _sceneService = sceneService ?? throw new ArgumentNullException(nameof(sceneService));
            _entityRepository = entityRepository ?? throw new ArgumentNullException(nameof(entityRepository));
        }

        public ButtonCommandId Id => ButtonCommandId.LoadMainMenuScene;

        public void Handle()
        {
            // SavedLevel savedLevel = _entityRepository.Get<SavedLevel>(ModelId.SavedLevel);
            // _sceneService.ChangeSceneAsync(
            //     ModelId.MainMenu, new ScenePayload(savedLevel.SavedLevelId, false, true));
        }
    }
}