using Sources.BoundedContexts.HealthBoosters.Domain;
using Sources.Frameworks.MVPPassiveView.Controllers.Interfaces.ControllerLifetimes;
using Sources.PresentationsInterfaces.Views.Constructors;

namespace Sources.Frameworks.YandexSdcFramework.Advertisings.Services.Interfaces
{
    public interface IAdvertisingService : IConstruct<HealthBooster>, IInitialize, IDestroy
    {
    }
}