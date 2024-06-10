using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using Sources.Frameworks.MVPPassiveView.Controllers.Interfaces.ControllerLifetimes;
using Sources.PresentationsInterfaces.Views.Constructors;

namespace Sources.Frameworks.YandexSdcFramework.Advertisings.Services.Interfaces
{
    public interface IAdvertisingService : IConstruct<PlayerWallet>, IInitialize, IDestroy
    {
    }
}