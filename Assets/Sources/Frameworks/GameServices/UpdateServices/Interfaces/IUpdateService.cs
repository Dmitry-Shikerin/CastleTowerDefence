using Sources.Frameworks.GameServices.ActionRegisters.Interfaces;
using Sources.Frameworks.GameServices.UpdateServices.Interfaces.Methods;
using Sources.InfrastructureInterfaces.Services.UpdateServices.Methods;

namespace Sources.Frameworks.GameServices.UpdateServices.Interfaces
{
    public interface IUpdateService : IUpdatable, IAllUnregister
    {
    }
}