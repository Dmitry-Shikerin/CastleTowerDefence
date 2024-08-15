using System;
using Doozy.Runtime.Signals;
using Sources.BoundedContexts.Huds.Presentations;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Constants;
using Sources.Frameworks.MyGameCreator.Achivements.Domain.Models;
using Sources.Frameworks.MyGameCreator.Achivements.Presentation;
using Sources.InfrastructureInterfaces.Services.Repositories;
using Zenject;

namespace Sources.Frameworks.MyGameCreator.Achivements.Infrastructure.Commands.Implementation.Base
{
    public class AchievementCommandBase : IAchievementCommand
    {
        private readonly DiContainer _container;
        
        private SignalStream _stream;
        private AchievementView _achievementView;

        public AchievementCommandBase(
            GameplayHud hud,
            DiContainer container)
        {
            if (hud == null)
                throw new ArgumentNullException(nameof(hud));
            
            _achievementView = hud.PopUpAchievementView ?? 
                               throw new ArgumentNullException(nameof(_achievementView));
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public virtual void Initialize()
        {
            _stream = SignalStream.Get(StreamConst.Gameplay, StreamConst.ReceivedAchievement);
        }

        public virtual void Execute()
        {
            _container.Inject(_achievementView);
            _stream.SendSignal(true);
            Destroy();
        }

        public virtual void Destroy()
        {
        }
    }
}