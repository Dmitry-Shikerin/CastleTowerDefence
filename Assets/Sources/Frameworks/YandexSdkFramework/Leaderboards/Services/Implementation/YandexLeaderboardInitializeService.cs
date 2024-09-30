using System;
using System.Collections.Generic;
using Agava.WebUtility;
using Sources.Domain.Models.Constants;
using Sources.Frameworks.YandexSdkFramework.Infrastructure.Factories.Views;
using Sources.Frameworks.YandexSdkFramework.Leaderboards.Domain.Constants;
using Sources.Frameworks.YandexSdkFramework.Leaderboards.Domain.Models;
using Sources.Frameworks.YandexSdkFramework.Leaderboards.Presentations.Implementation.Views;
using Sources.Frameworks.YandexSdkFramework.Leaderboards.Services.Interfaces;
using YG;
using YG.Utils.LB;

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
            
            if (WebApplication.IsRunningOnWebGL == false)
                return;

            //TODO закоментил
            
            if (YandexGame.auth == false)
                return;
            
            YandexGame.GetLeaderboard(
                LeaderBoardNameConst.Leaderboard, 
                5, 
                5, 
                5, 
                "medium");
            YandexGame.onGetLeaderboard += OnGetLeaderboard;
        }

        private void OnGetLeaderboard(LBData result)
        {
            var count = result.entries.Length < _leaderBoardElementViews.Count
                ? result.entries.Length
                : _leaderBoardElementViews.Count;

            for (var i = 0; i < count; i++)
            {
                var rank = result.players[i].rank;
                var score = result.players[i].score;
                var name = result.players[i].name;

                if (string.IsNullOrEmpty(name))
                    name = YandexGame.lang switch
                    {
                        LocalizationConst.English => AnonymousConst.English,
                        LocalizationConst.Turkish => AnonymousConst.Turkish,
                        LocalizationConst.Russian => AnonymousConst.Russian,
                        _ => AnonymousConst.English
                    };

                _leaderBoardElementViewFactory.Create(
                    new LeaderBoardPlayer(rank, name, score),
                    _leaderBoardElementViews[i]);
            }
            
            //TODO порефакторить
            YandexGame.onGetLeaderboard -= OnGetLeaderboard;
        }
    }
}