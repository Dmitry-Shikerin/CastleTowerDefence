using System;

namespace Sources.BoundedContexts.HealthBoosters.Domain
{
    public class HealthBuster
    {
        private int _count;

        public event Action CountChanged;
        
        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                CountChanged?.Invoke();
            }
        }

        private bool TryApply()
        {
            if (Count == 0)
                return false;

            if (Count < 0)
                throw new ArgumentOutOfRangeException();
            
            Count--;
            return true;
        }
    }
}