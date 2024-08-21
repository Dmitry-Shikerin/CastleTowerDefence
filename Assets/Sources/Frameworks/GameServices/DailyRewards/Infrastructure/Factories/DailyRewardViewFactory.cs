using System;
using JetBrains.Annotations;
using MyAudios.MyUiFramework.Utils.Soundies.Infrastructure;
using Sources.Frameworks.GameServices.DailyRewards.Controllers;
using Sources.Frameworks.GameServices.DailyRewards.Presentation;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces;
using Sources.Frameworks.GameServices.ServerTimes.Services;
using Sources.InfrastructureInterfaces.Services.Repositories;

namespace Sources.Frameworks.GameServices.DailyRewards.Infrastructure.Factories
{
    public class DailyRewardViewFactory
    {
        private readonly IEntityRepository _entityRepository;
        private readonly IServerTimeService _serverTimeService;
        private readonly ISoundyService _soundyService;
        private readonly ILoadService _loadService;

        public DailyRewardViewFactory(
            IEntityRepository entityRepository,
            IServerTimeService serverTimeService,
            ISoundyService soundyService,
            ILoadService loadService)
        {
            _entityRepository = entityRepository ?? throw new ArgumentNullException(nameof(entityRepository));
            _serverTimeService = serverTimeService ?? throw new ArgumentNullException(nameof(serverTimeService));
            _soundyService = soundyService ?? throw new ArgumentNullException(nameof(soundyService));
            _loadService = loadService ?? throw new ArgumentNullException(nameof(loadService));
        }

        public DailyRewardView Create(DailyRewardView view)
        {
            DailyRewardPresenter presenter = new DailyRewardPresenter(
                _entityRepository, 
                view, 
                _serverTimeService,
                _soundyService,
                _loadService);
            view.Construct(presenter);
            
            return view;
        }
    }
}