using System;
using Sources.Frameworks.Domain.Interfaces.Entities;

namespace Sources.BoundedContexts.HealthBoosters.Domain
{
    public class HealthBooster : IEntity
    {
        private int _amount;

        public event Action CountChanged;
        public event Action CountRemoved;

        public string Id { get; set; }
        public Type Type => GetType();

        public int Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                CountChanged?.Invoke();
            }
        }

        public bool TryApply()
        {
            if (Amount == 0)
                return false;

            if (Amount < 0)
                throw new ArgumentOutOfRangeException();
            
            Amount--;
            CountRemoved?.Invoke();
            
            return true;
        }
    }
}