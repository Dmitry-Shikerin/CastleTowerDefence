using Agava.WebUtility;
using Agava.YandexGames;
using Sources.Domain.Models.Constants;
using Sources.InfrastructureInterfaces.Services.YandexSDKServices;

namespace Sources.Frameworks.YandexSdcFramework.Services.Leaderboards
{
    public class YandexLeaderBoardScoreSetter : ILeaderBoardScoreSetter
    {
        public void SetPlayerScore(int score)
        {
            if (WebApplication.IsRunningOnWebGL == false)
                return;

            if (PlayerAccount.IsAuthorized == false)
                return;
            
            Leaderboard.GetPlayerEntry(LeaderBoardNameConst.Leaderboard, result =>
            {
                if (result.score < score)
                    Leaderboard.SetScore(LeaderBoardNameConst.Leaderboard, score);
            });
        }
    }
}