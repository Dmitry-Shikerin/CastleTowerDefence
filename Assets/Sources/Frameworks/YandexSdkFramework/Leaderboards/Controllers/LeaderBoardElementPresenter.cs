using System;
using Sources.Frameworks.MVPPassiveView.Controllers.Implementation;
using Sources.Frameworks.YandexSdkFramework.Leaderboards.Domain.Models;
using Sources.Frameworks.YandexSdkFramework.Leaderboards.Presentations.Interfaces.Views;

namespace Sources.Frameworks.YandexSdkFramework.Leaderboards.Controllers
{
    public class LeaderBoardElementPresenter : PresenterBase
    {
        private readonly LeaderBoardPlayer _leaderBoardPlayer;
        private readonly ILeaderBoardElementView _leaderboardElementView;

        public LeaderBoardElementPresenter(
            LeaderBoardPlayer leaderBoardPlayer, 
            ILeaderBoardElementView leaderboardElementView)
        {
            _leaderBoardPlayer = leaderBoardPlayer ?? throw new ArgumentNullException(nameof(leaderBoardPlayer));
            _leaderboardElementView = leaderboardElementView ??
                                      throw new ArgumentNullException(nameof(leaderboardElementView));
        }

        public override void Enable()
        {
            _leaderboardElementView.SetName(_leaderBoardPlayer.Name);
            _leaderboardElementView.SetRank(_leaderBoardPlayer.Rank.ToString());
            _leaderboardElementView.SetScore(_leaderBoardPlayer.Score.ToString());
        }
    }
}