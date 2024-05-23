using System;

namespace Sources.DomainInterfaces.Models.Gameplay
{
    public interface ILevel
    {
        event Action Completed;
        
        public bool IsCompleted { get; }
    }
}