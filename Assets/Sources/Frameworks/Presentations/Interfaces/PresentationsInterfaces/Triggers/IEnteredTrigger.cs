using System;

namespace Sources.PresentationsInterfaces.Triggers
{
    public interface IEnteredTrigger<out T>
    {
        public event Action<T> Entered;
    }
}