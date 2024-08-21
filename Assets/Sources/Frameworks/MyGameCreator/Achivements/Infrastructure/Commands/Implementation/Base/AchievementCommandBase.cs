using System;
using System.Linq;
using Doozy.Runtime.Signals;
using JetBrains.Annotations;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Constants;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces;
using Sources.Frameworks.GameServices.Prefabs.Implementation;
using Sources.Frameworks.MyGameCreator.Achivements.Domain.Configs;
using Sources.Frameworks.MyGameCreator.Achivements.Domain.Models;
using Sources.Frameworks.MyGameCreator.Achivements.Presentation;
using TeoGames.Mesh_Combiner.Scripts.Extension;
using UnityEngine;
using Zenject;

namespace Sources.Frameworks.MyGameCreator.Achivements.Infrastructure.Commands.Implementation.Base
{
    public class AchievementCommandBase : IAchievementCommand
    {
        private readonly DiContainer _container;
        
        private SignalStream _stream;
        private AchievementView _achievementView;
        private readonly IPrefabCollector _prefabCollector;
        private readonly ILoadService _loadService;

        public AchievementCommandBase(
            AchievementView achievementView,
            IPrefabCollector prefabCollector,
            ILoadService loadService,
            DiContainer container)
        {
            _achievementView = achievementView ?? 
                               throw new ArgumentNullException(nameof(achievementView));
            _prefabCollector = prefabCollector ?? throw new ArgumentNullException(nameof(prefabCollector));
            _loadService = loadService ?? throw new ArgumentNullException(nameof(loadService));
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public virtual void Initialize()
        {
            _stream = SignalStream.Get(StreamConst.Gameplay, StreamConst.ReceivedAchievement);
        }

        public virtual void Execute(Achievement achievement)
        {
            AchievementConfig config =_prefabCollector
                .Get<AchievementConfigCollector>()
                .Configs
                .First(config => config.Id == achievement.Id);
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