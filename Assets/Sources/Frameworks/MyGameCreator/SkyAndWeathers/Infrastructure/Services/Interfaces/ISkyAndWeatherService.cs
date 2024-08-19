using Sources.Frameworks.MVPPassiveView.Controllers.Interfaces.ControllerLifetimes;

namespace Sources.Frameworks.MyGameCreator.SkyAndWeathers.Infrastructure.Services.Implementation
{
    public interface ISkyAndWeatherService : IInitialize, IDestroy
    {
    }
}