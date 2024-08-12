using Sources.Frameworks.MVPPassiveView.Controllers.Interfaces.ControllerLifetimes;
using Sources.Frameworks.MyGameCreator.Achivements.Domain.Configs;

namespace Sources.Frameworks.MyGameCreator.Achivements.Infrastructure.Services.Interfaces
{
    public interface IAchievementService : IInitialize, IDestroy
    {
        AchievementConfig GetConfig(string id);
        void Register();
    }
}