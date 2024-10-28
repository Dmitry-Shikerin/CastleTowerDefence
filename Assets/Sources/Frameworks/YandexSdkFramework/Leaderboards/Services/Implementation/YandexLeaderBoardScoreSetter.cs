using Agava.WebUtility;
using Sources.Frameworks.YandexSdkFramework.Leaderboards.Domain.Constants;
using Sources.Frameworks.YandexSdkFramework.Leaderboards.Services.Interfaces;
using UnityEngine.SocialPlatforms.Impl;
using YG;

namespace Sources.Frameworks.YandexSdkFramework.Leaderboards.Services.Implementation
{
    public class YandexLeaderBoardScoreSetter : ILeaderBoardScoreSetter
    {
        public void SetPlayerScore(int score)
        {
            if (WebApplication.IsRunningOnWebGL == false)
                return;

            //TODO закоментил
            if (YandexGame.auth == false)
                return;
            
            YandexGame.NewLeaderboardScores(LeaderBoardNameConst.Leaderboard, score);
            // Leaderboard.GetPlayerEntry(LeaderBoardNameConst.Leaderboard, result =>
            // {
            //     if (result.score < score)
            //         Leaderboard.SetScore(LeaderBoardNameConst.Leaderboard, score);
            // });
        }
    }
}