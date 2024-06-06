using System;
using Sources.Controllers.YandexSDK;
using Sources.Domain.Models.YandexSDK;
using Sources.Frameworks.YandexSdcFramework.Presentations.Views;
using Sources.Infrastructure.Factories.Controllers.YandexSDK;
using Sources.PresentationsInterfaces.Views.YandexSDK;

namespace Sources.Infrastructure.Factories.Views.YandexSDK
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