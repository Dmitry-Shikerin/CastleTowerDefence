using System;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.Frameworks.Domain.Interfaces.Entities;

namespace Sources.BoundedContexts.Tutorials.Domain.Models
{
    public class Tutorial : IEntity
    {
        private bool _hasCompleted;

        public Tutorial()
            : this(ModelId.Tutorial, false)
        {
        }

        private Tutorial(string id, bool hasCompleted)
        {
            Id = id;
            HasCompleted = hasCompleted;
        }

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

        public string Id { get; }
        public Type Type => GetType();
    }
}