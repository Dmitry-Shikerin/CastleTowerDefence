using Sources.BoundedContexts.Players.Domain;
using Sources.ControllersInterfaces.ControllerLifetimes;
using Sources.InfrastructureInterfaces.Services.StatesLifetimes;
using Sources.PresentationsInterfaces.Views.Constructors;

namespace Sources.Frameworks.YandexSdcFramework.ServicesInterfaces.AdverticingServices
{
    public interface IAdvertisingService : IConstruct<PlayerWallet>, IEnable, IDisable
    {
    }
}