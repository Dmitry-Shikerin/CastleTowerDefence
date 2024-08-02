using Sources.Frameworks.GameServices.UpdateServices.Interfaces.Methods;
using Sources.Frameworks.MVPPassiveView.Controllers.Interfaces.ControllerLifetimes;
using Sources.PresentationsInterfaces.Views.Constructors;
using Zenject;

namespace Sources.ECSBoundedContexts.StarUps.Interfaces
{
    public interface IEcsGameStartUp : IInitialize, IUpdatable, IDestroy
    {
    }
}