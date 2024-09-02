using System;
using Sources.Frameworks.YandexSdkFramework.Infrastructure.Factories.Controllers;
using Sources.Frameworks.YandexSdkFramework.Leaderboards.Controllers;
using Sources.Frameworks.YandexSdkFramework.Leaderboards.Domain.Models;
using Sources.Frameworks.YandexSdkFramework.Leaderboards.Presentations.Implementation.Views;
using Sources.Frameworks.YandexSdkFramework.Leaderboards.Presentations.Interfaces.Views;

namespace Sources.Frameworks.YandexSdkFramework.Infrastructure.Factories.Views
{
    public class LeaderBoardElementViewFactory
    {
        private readonly LeaderBoardElementPresenterFactory _leaderBoardElementPresenterFactory;

        public LeaderBoardElementViewFactory(LeaderBoardElementPresenterFactory leaderBoardElementPresenterFactory)
        {
            _leaderBoardElementPresenterFactory = leaderBoardElementPresenterFactory ??
                                                  throw new ArgumentNullException(nameof(leaderBoardElementPresenterFactory));
        }

        public ILeaderBoardElementView Create(
            LeaderBoardPlayer leaderBoardPlayer,
            LeaderBoardElementView leaderBoardElementView)
        {
            if (leaderBoardPlayer == null)
                throw new ArgumentNullException(nameof(leaderBoardPlayer));
            
            if (leaderBoardElementView == null)
                throw new ArgumentNullException(nameof(leaderBoardElementView));

            LeaderBoardElementPresenter leaderBoardElementPresenter =
                _leaderBoardElementPresenterFactory.Create(leaderBoardPlayer, leaderBoardElementView);

            leaderBoardElementView.Construct(leaderBoardElementPresenter);

            return leaderBoardElementView;
        }
    }
}