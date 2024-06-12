using System.Collections.Generic;
using Sources.Frameworks.YandexSdcFramework.Presentations.Views;
using Sources.PresentationsInterfaces.Views.Constructors;

namespace Sources.Frameworks.YandexSdkFramework.Leaderboards.Services.Interfaces
{
    public interface ILeaderboardInitializeService : IConstruct<IReadOnlyList<LeaderBoardElementView>>
    {
        void Fill();
    }
}