using Sources.Frameworks.MVPPassiveView.Controllers.Interfaces.ControllerLifetimes;
using Sources.Frameworks.MyGameCreator.Achievements.Domain.Models;

namespace Sources.Frameworks.MyGameCreator.Achievements.Infrastructure.Commands.Interfaces
{
    public interface IAchievementCommand : IInitialize, IDestroy
    {
        void Execute(Achievement achievement);
    }
}