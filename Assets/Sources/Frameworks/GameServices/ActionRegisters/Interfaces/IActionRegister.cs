using System;

namespace Sources.Frameworks.GameServices.ActionRegisters.Interfaces
{
    public interface IActionRegister<out T>
    {
        void Register(Action<T> action);
        void UnRegister(Action<T> action);
    }
}