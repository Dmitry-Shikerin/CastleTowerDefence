using System;
using System.Linq;
using Doozy.Runtime.Signals;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Constants;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces;
using Sources.Frameworks.GameServices.Prefabs.Interfaces;
using Sources.Frameworks.MyGameCreator.Achievements.Domain.Configs;
using Sources.Frameworks.MyGameCreator.Achievements.Domain.Models;
using Sources.Frameworks.MyGameCreator.Achievements.Infrastructure.Commands.Interfaces;
using Sources.Frameworks.MyGameCreator.Achievements.Presentation;
using Zenject;

namespace Sources.Frameworks.MyGameCreator.Achievements.Infrastructure.Commands.Implementation.Base
{
    public class AchievementCommandBase : IAchievementCommand
    {
        private readonly DiContainer _container;
        private readonly AchievementView _achievementView;
        
        private SignalStream _stream;
        private readonly IAssetCollector _assetCollector;
        private readonly ILoadService _loadService;

        public AchievementCommandBase(
            AchievementView achievementView,
            IAssetCollector assetCollector,
            ILoadService loadService,
            DiContainer container)
        {
            _achievementView = achievementView ?? 
                               throw new ArgumentNullException(nameof(achievementView));
            _assetCollector = assetCollector ?? throw new ArgumentNullException(nameof(assetCollector));
            _loadService = loadService ?? throw new ArgumentNullException(nameof(loadService));
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public virtual void Initialize()
        {
            _stream = SignalStream.Get(StreamConst.Gameplay, StreamConst.ReceivedAchievement);
        }

        public virtual void Execute(Achievement achievement)
        {
            AchievementConfig config =_assetCollector
                .Get<AchievementConfigCollector>()
                .Configs
                .First(config => config.Id == achievement.Id);

            achievement.IsCompleted = true;
            _container.Inject(_achievementView);
            _loadService.Save(achievement);
            _stream.SendSignal(true);
            _achievementView.Construct(achievement, config);
            
        }

        public virtual void Destroy()
        {
        }
    }
}