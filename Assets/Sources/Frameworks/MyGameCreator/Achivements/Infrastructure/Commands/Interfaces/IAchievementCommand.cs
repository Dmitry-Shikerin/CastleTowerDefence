using Sources.Frameworks.MVPPassiveView.Controllers.Interfaces.ControllerLifetimes;
using Sources.Frameworks.MyGameCreator.Achivements.Domain.Models;

namespace Sources.Frameworks.MyGameCreator.Achivements.Infrastructure.Commands
{
    public interface IAchievementCommand : IInitialize, IDestroy
    {
        void Execute(Achievement achievement);
    }
}