using System;

namespace Sources.Frameworks.MyGameCreator.Triggers.Interfaces
{
    public interface IEnteredTrigger<out T>
    {
        public event Action<T> Entered;
    }
}