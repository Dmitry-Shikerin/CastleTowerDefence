using Sources.Frameworks.MVPPassiveView.Controllers.Interfaces.ControllerLifetimes;

namespace Sources.Frameworks.MyGameCreator.DailyRewards.Infrastructure.Services.Interfaces
{
    public interface IDailyRewardService : IInitialize, IDestroy
    {
    }
}