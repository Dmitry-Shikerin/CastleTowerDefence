using System;
using Sources.Frameworks.GameServices.DailyRewards.Controllers;
using Sources.Frameworks.GameServices.DailyRewards.Presentation;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;
using Sources.Frameworks.GameServices.ServerTimes.Services;
using Sources.Frameworks.GameServices.ServerTimes.Services.Interfaces;

namespace Sources.Frameworks.GameServices.DailyRewards.Infrastructure.Factories
{
    public class DailyRewardViewFactory
    {
        private readonly IEntityRepository _entityRepository;
        private readonly ITimeService _timeService;
        private readonly ILoadService _loadService;

        public DailyRewardViewFactory(
            IEntityRepository entityRepository,
            ITimeService timeService,
            ILoadService loadService)
        {
            _entityRepository = entityRepository ?? throw new ArgumentNullException(nameof(entityRepository));
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
            _loadService = loadService ?? throw new ArgumentNullException(nameof(loadService));
        }

        public DailyRewardView Create(DailyRewardView view)
        {
            DailyRewardPresenter presenter = new DailyRewardPresenter(
                _entityRepository, 
                view, 
                _timeService,
                _loadService);
            view.Construct(presenter);
            
            return view;
        }
    }
}