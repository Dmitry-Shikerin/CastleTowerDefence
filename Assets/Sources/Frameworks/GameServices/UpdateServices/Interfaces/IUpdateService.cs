using Sources.Frameworks.GameServices.ActionRegisters.Interfaces;
using Sources.Frameworks.GameServices.UpdateServices.Interfaces.Methods;

namespace Sources.Frameworks.GameServices.UpdateServices.Interfaces
{
    public interface IUpdateService : IUpdatable, IAllUnregister
    {
    }
}