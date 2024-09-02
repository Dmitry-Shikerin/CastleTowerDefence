using System;
using System.Collections.Generic;
using Agava.WebUtility;
using Agava.YandexGames;
using Sources.Domain.Models.Constants;
using Sources.Frameworks.YandexSdkFramework.Infrastructure.Factories.Views;
using Sources.Frameworks.YandexSdkFramework.Leaderboards.Domain.Constants;
using Sources.Frameworks.YandexSdkFramework.Leaderboards.Domain.Models;
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
            
            if (WebApplication.IsRunningOnWebGL == false)
                return;

            if (PlayerAccount.IsAuthorized == false)
                return;
            
            Leaderboard.GetEntries(LeaderBoardNameConst.Leaderboard, result =>
            {
                var count = result.entries.Length < _leaderBoardElementViews.Count
                    ? result.entries.Length
                    : _leaderBoardElementViews.Count;
                
                for (var i = 0; i < count; i++)
                {
                    var rank = result.entries[i].rank;
                    var score = result.entries[i].score;
                    var name = result.entries[i].player.publicName;

                    if (string.IsNullOrEmpty(name))
                        name = YandexGamesSdk.Environment.i18n.lang switch
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
            });
        }
    }
}