using System;
using Sources.BoundedContexts.Upgrades.Controllers;
using Sources.BoundedContexts.Upgrades.Presentation.Implementation;
using Sources.BoundedContexts.Upgrades.Presentation.Interfaces;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;

namespace Sources.BoundedContexts.Upgrades.Infrastructure.Factories.Views
{
    public class UpgradeViewFactory
    {
        private readonly IEntityRepository _entityRepository;

        public UpgradeViewFactory(IEntityRepository entityRepository)
        {
            _entityRepository = entityRepository ?? 
                                throw new ArgumentNullException(nameof(entityRepository));
        }

        public IUpgradeView Create(string upgradeId, UpgradeView view)
        {
            UpgradePresenter presenter = new UpgradePresenter(_entityRepository, upgradeId, view);
            view.Construct(presenter);
            
            return view;
        }
    }
}