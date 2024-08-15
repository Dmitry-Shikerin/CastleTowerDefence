using System;
using Sources.Frameworks.Domain.Interfaces.Entities;
using UnityEngine;

namespace Sources.Frameworks.MyGameCreator.Achivements.Domain.Models
{
    public class Achievement : IEntity
    {
        private bool _isCompleted;

        public Achievement(string id)
        {
            Id = id;
        }

        public event Action Completed;
        
        public string Id { get; }
        public Type Type => GetType();

        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                if (_isCompleted)
                    return;
                
                _isCompleted = value;
                Completed?.Invoke();
            }
        }
    }
}