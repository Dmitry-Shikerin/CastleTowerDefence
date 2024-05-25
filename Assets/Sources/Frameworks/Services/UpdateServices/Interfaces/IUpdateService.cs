using Sources.InfrastructureInterfaces.Services.Registers;
using Sources.InfrastructureInterfaces.Services.UpdateServices.Methods;

namespace Sources.InfrastructureInterfaces.Services.UpdateServices
{
    public interface IUpdateService : IUpdatable, IAllUnregister
    {
    }
}