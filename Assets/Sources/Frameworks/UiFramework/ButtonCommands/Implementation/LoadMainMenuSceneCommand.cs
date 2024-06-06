using System;
using Sources.Frameworks.UiFramework.ButtonProviders.Domain;
using Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Commands.Interfaces;
using Sources.Frameworks.UiFramework.Domain.Commands;
using Sources.InfrastructureInterfaces.Services.Repositories;
using Sources.InfrastructureInterfaces.Services.SceneServices;

namespace Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Commands.Implementation
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