using System;
using Doozy.Runtime.Signals;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Constants;
using Sources.Frameworks.MyGameCreator.Achivements.Domain.Models;
using Sources.Frameworks.MyGameCreator.Achivements.Presentation;
using Zenject;

namespace Sources.Frameworks.MyGameCreator.Achivements.Infrastructure.Commands.Implementation.Base
{
    public class AchievementCommandBase : IAchievementCommand
    {
        private readonly DiContainer _container;
        
        private SignalStream _stream;
        private AchievementView _achievementView;

        public AchievementCommandBase(
            AchievementView achievementView,
            DiContainer container)
        {
            _achievementView = achievementView ?? 
                               throw new ArgumentNullException(nameof(achievementView));
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public virtual void Initialize()
        {
            _stream = SignalStream.Get(StreamConst.Gameplay, StreamConst.ReceivedAchievement);
        }

        public virtual void Execute(Achievement achievement)
        {
            _container.Inject(_achievementView);
            _stream.SendSignal(true);
            _achievementView.Construct(achievement);
        }

        public virtual void Destroy()
        {
        }
    }
}