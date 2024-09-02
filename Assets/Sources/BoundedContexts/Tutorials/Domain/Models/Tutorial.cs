using System;
using Sources.Frameworks.Domain.Interfaces.Entities;

namespace Sources.BoundedContexts.Tutorials.Domain.Models
{
    public class Tutorial : IEntity
    {
        private bool _hasCompleted;

        public event Action OnCompleted;

        public bool HasCompleted
        {
            get => _hasCompleted;
            set
            {
                if (_hasCompleted == value)
                    return;
                
                _hasCompleted = value;
                OnCompleted?.Invoke();
            }
        }

        public string Id { get; set; }
        public Type Type => GetType();
    }
}