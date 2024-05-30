using System;
using Sources.InfrastructureInterfaces.Services.Registers;

namespace Sources.InfrastructureInterfaces.Services.UpdateServices
{
    public interface IUpdateRegister : IActionRegister<float>
    {
        event Action<float> UpdateChanged;
    }
}