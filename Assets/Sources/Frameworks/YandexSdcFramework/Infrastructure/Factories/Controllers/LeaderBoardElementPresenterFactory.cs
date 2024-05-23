using System;
using Sources.Controllers.YandexSDK;
using Sources.Domain.Models.YandexSDK;
using Sources.PresentationsInterfaces.Views.YandexSDK;

namespace Sources.Infrastructure.Factories.Controllers.YandexSDK
{
    public class LeaderBoardElementPresenterFactory
    {
        public LeaderBoardElementPresenter Create(
            LeaderBoardPlayer leaderBoardPlayer,
            ILeaderBoardElementView leaderBoardElementView)
        {
            if (leaderBoardPlayer == null)
                throw new ArgumentNullException(nameof(leaderBoardPlayer));
            if (leaderBoardElementView == null)
                throw new ArgumentNullException(nameof(leaderBoardElementView));

            return new LeaderBoardElementPresenter(leaderBoardPlayer, leaderBoardElementView);
        }
    }
}