using Sources.Frameworks.MVPPassiveView.Controllers.Interfaces.ControllerLifetimes;

namespace Sources.Frameworks.MyGameCreator.Achivements.Infrastructure.Commands
{
    public interface IAchievementCommand : IInitialize, IExecutable, IDestroy
    {
    }
}