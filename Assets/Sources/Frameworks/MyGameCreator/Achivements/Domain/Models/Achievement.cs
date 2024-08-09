using System;
using Sources.Frameworks.Domain.Interfaces.Entities;

namespace Sources.Frameworks.MyGameCreator.Achivements.Domain
{
    public class Achievement : IEntity
    {
        private bool _isCompleted;
        private bool _isCompleted1;

        public Achievement(string id)
        {
            Id = id;
        }

        public event Action Completed;
        
        public string Id { get; }
        public Type Type => GetType();

        public bool IsCompleted
        {
            get => _isCompleted1;
            set
            {
                if (_isCompleted)
                    return;
                
                _isCompleted1 = value;
                Completed?.Invoke();
            }
        }
    }
}