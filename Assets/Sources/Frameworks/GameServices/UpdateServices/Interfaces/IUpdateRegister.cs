using System;
using Sources.Frameworks.GameServices.ActionRegisters.Interfaces;

namespace Sources.Frameworks.GameServices.UpdateServices.Interfaces
{
    public interface IUpdateRegister : IActionRegister<float>
    {
        event Action<float> UpdateChanged;
    }
}