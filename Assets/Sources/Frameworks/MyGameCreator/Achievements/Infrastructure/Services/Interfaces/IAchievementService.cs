using Sources.Frameworks.MVPPassiveView.Controllers.Interfaces.ControllerLifetimes;
using Sources.Frameworks.MyGameCreator.Achievements.Domain.Configs;

namespace Sources.Frameworks.MyGameCreator.Achievements.Infrastructure.Services.Interfaces
{
    public interface IAchievementService : IInitialize, IDestroy
    {
        AchievementConfig GetConfig(string id);
        void Register();
    }
}