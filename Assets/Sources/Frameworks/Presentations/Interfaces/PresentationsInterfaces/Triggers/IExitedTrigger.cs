using System;

namespace Sources.PresentationsInterfaces.Triggers
{
    public interface IExitedTrigger<out T>
    {
        public event Action<T> Exited;
    }
}