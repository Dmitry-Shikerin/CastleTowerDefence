using Agava.WebUtility;
using Sources.Frameworks.YandexSdkFramework.Leaderboards.Services.Interfaces;

namespace Sources.Frameworks.YandexSdkFramework.Leaderboards.Services.Implementation
{
    public class YandexLeaderBoardScoreSetter : ILeaderBoardScoreSetter
    {
        public void SetPlayerScore(int score)
        {
            if (WebApplication.IsRunningOnWebGL == false)
                return;

            //TODO закоментил
            // if (PlayerAccount.IsAuthorized == false)
            //     return;
            //
            // Leaderboard.GetPlayerEntry(LeaderBoardNameConst.Leaderboard, result =>
            // {
            //     if (result.score < score)
            //         Leaderboard.SetScore(LeaderBoardNameConst.Leaderboard, score);
            // });
        }
    }
}