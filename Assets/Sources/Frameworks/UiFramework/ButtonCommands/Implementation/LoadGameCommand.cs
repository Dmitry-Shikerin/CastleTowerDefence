using System;
using Sources.BoundedContexts.Ids;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.Frameworks.GameServices.Scenes.Services.Interfaces;
using Sources.Frameworks.UiFramework.ButtonProviders.Domain;
using Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Commands.Interfaces;
using Sources.Frameworks.UiFramework.Domain.Commands;
using Sources.InfrastructureInterfaces.Services.LoadServices;
using Sources.InfrastructureInterfaces.Services.Repositories;

namespace Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Commands.Implementation
{
    public class LoadGameCommand : IButtonCommand
    {
        private readonly ILoadService _loadService;
        private readonly IEntityRepository _entityRepository;
        private readonly ISceneService _sceneService;

        public LoadGameCommand(
            ILoadService loadService,
            IEntityRepository entityRepository,
            ISceneService sceneService)
        {
            _loadService = loadService ?? throw new ArgumentNullException(nameof(loadService));
            _entityRepository = entityRepository ?? throw new ArgumentNullException(nameof(entityRepository));
            _sceneService = sceneService ?? throw new ArgumentNullException(nameof(sceneService));
        }

        public ButtonCommandId Id => ButtonCommandId.LoadGame;

        public void Handle()
        {
            if (_loadService.HasKey(ModelId.PlayerWallet) == false)
                return;

            // SavedLevel savedLevel = _entityRepository.Get<SavedLevel>(ModelId.SavedLevel);

            // _sceneService.ChangeSceneAsync(
            //     savedLevel.SavedLevelId,
            //     new ScenePayload(savedLevel.SavedLevelId, true, false));
        }
    }
}