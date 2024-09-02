using Sources.Frameworks.MVPPassiveView.Controllers.Interfaces.ControllerLifetimes;

namespace Sources.Frameworks.MVPPassiveView.Controllers.Interfaces.Presenters
{
    public interface IPresenter : IInitialize, IEnable, IDisable, IDestroy
    {
    }
}