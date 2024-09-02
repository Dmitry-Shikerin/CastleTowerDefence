using System;

namespace Sources.Frameworks.MyGameCreator.Triggers.Interfaces
{
    public interface IExitedTrigger<out T>
    {
        public event Action<T> Exited;
    }
}