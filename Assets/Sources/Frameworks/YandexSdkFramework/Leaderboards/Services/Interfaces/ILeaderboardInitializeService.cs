using System.Collections.Generic;
using Sources.Frameworks.MVPPassiveView.Controllers.Interfaces.ControllerLifetimes;
using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.Views.Constructors;
using Sources.Frameworks.YandexSdkFramework.Leaderboards.Presentations.Implementation.Views;

namespace Sources.Frameworks.YandexSdkFramework.Leaderboards.Services.Interfaces
{
    public interface ILeaderboardInitializeService : IInitialize, IDestroy, IConstruct<IReadOnlyList<LeaderBoardElementView>>
    {
        void Fill();
    }
}