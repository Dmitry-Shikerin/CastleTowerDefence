using System;
using Sources.Frameworks.YandexSdkFramework.Leaderboards.Controllers;
using Sources.Frameworks.YandexSdkFramework.Leaderboards.Domain.Models;
using Sources.Frameworks.YandexSdkFramework.Leaderboards.Presentations.Interfaces.Views;

namespace Sources.Frameworks.YandexSdkFramework.Infrastructure.Factories.Controllers
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