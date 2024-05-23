using System;

namespace Sources.InfrastructureInterfaces.Services.Registers
{
    public interface IActionRegister<out T>
    {
        void Register(Action<T> action);
        void UnRegister(Action<T> action);
    }
}