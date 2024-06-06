using System;
using System.Collections.Generic;
using Agava.WebUtility;
using Agava.YandexGames;
using Sources.BoundedContexts.Huds.Presentations;
using Sources.Domain.Models.Constants;
using Sources.Domain.Models.YandexSDK;
using Sources.Frameworks.YandexSdcFramework.Presentations.Views;
using Sources.Frameworks.YandexSdcFramework.ServicesInterfaces.Leaderboads;
using Sources.Infrastructure.Factories.Views.YandexSDK;

namespace Sources.Frameworks.YandexSdcFramework.Services.Leaderboards
{
    public class YandexLeaderboardInitializeService : ILeaderboardInitializeService
    {
        private readonly LeaderBoardElementViewFactory _leaderBoardElementViewFactory;
        private IReadOnlyList<LeaderBoardElementView> _leaderBoardElementViews;
        
        public YandexLeaderboardInitializeService(
            MainMenuHud mainMenuHud,
            LeaderBoardElementViewFactory leaderBoardElementViewFactory)
        {
            if (mainMenuHud == null)
                throw new ArgumentNullException(nameof(mainMenuHud));

            _leaderBoardElementViews = mainMenuHud.LeaderBoardElementViews;
            
            _leaderBoardElementViewFactory = leaderBoardElementViewFactory ??
                                             throw new ArgumentNullException(nameof(leaderBoardElementViewFactory));
        }
        
        public void Fill()
        {
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