using System;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using Sources.BoundedContexts.Upgrades.Controllers;
using Sources.BoundedContexts.Upgrades.Domain.Models;
using Sources.BoundedContexts.Upgrades.Presentation.Interfaces;
using Sources.InfrastructureInterfaces.Services.Repositories;

namespace Sources.BoundedContexts.Upgrades.Infrastructure.Factories.Controllers
{
    public class UpgradePresenterFactory
    {
        private readonly IEntityRepository _entityRepository;

        public UpgradePresenterFactory(IEntityRepository entityRepository)
        {
            _entityRepository = entityRepository ?? 
                                throw new ArgumentNullException(nameof(entityRepository));
        }

        public UpgradePresenter Create(string upgradeId, IUpgradeView view)
        {
            return new UpgradePresenter(_entityRepository, upgradeId, view);
        }
    }
}