using System;
using System.Collections.Generic;
using Sources.Frameworks.YandexSdkFramework.Infrastructure.Factories.Views;
using Sources.Frameworks.YandexSdkFramework.Leaderboards.Presentations.Implementation.Views;
using Sources.Frameworks.YandexSdkFramework.Leaderboards.Services.Interfaces;

namespace Sources.Frameworks.YandexSdkFramework.Leaderboards.Services.Implementation
{
    public class YandexLeaderboardInitializeService : ILeaderboardInitializeService
    {
        private readonly LeaderBoardElementViewFactory _leaderBoardElementViewFactory;
        private IReadOnlyList<LeaderBoardElementView> _leaderBoardElementViews;
        
        public YandexLeaderboardInitializeService(
            LeaderBoardElementViewFactory leaderBoardElementViewFactory)
        {
            
            _leaderBoardElementViewFactory = leaderBoardElementViewFactory ??
                                             throw new ArgumentNullException(nameof(leaderBoardElementViewFactory));
        }

        public void Construct(IReadOnlyList<LeaderBoardElementView> leaderBoardElementViews) =>
            _leaderBoardElementViews = leaderBoardElementViews ?? 
                                       throw new ArgumentNullException(nameof(leaderBoardElementViews));

        public void Fill()
        {
            if(_leaderBoardElementViews == null)
                throw new NullReferenceException(nameof(_leaderBoardElementViews));
        }

        public void Initialize()
        {
        }

        public void Destroy()
        {
        }
    }
}