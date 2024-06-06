using System;
using System.Collections.Generic;
using Sources.Frameworks.GameServices.ActionRegisters.Interfaces;

namespace Sources.Frameworks.GameServices.ActionRegisters.Implementation
{
    public abstract class ActionRegisterer<T> : IActionRegister<T>
    {
        protected readonly List<Action<T>> Actions = new List<Action<T>>();
        
        public void UnRegister(Action<T> action) => 
            Actions.Remove(action);
        
        public void Register(Action<T> action) => 
            Actions.Add(action);
        
        public void UnregisterAll() => 
            Actions.Clear();
    }
}