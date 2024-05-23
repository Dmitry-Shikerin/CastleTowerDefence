using System;
using System.Collections;
using System.Collections.Generic;

namespace Sources.Utils.CustomCollections
{
    public class CustomCollection<T> : ICustomCollection<T>
    {
        private List<T> _collection = new List<T>();

        public event Action CountChanged;
        public event Action Added;
        public event Action Removed;

        public int Count => _collection.Count;

        public T this[int index] => _collection[index];

        public void Add(T item)
        {
            _collection.Add(item);
            Added?.Invoke();
            CountChanged?.Invoke();
        }

        public bool Remove(T item)
        {
            bool result = _collection.Remove(item);
            Removed?.Invoke();
            CountChanged?.Invoke();
            return result;
        }

        public void Clear() =>
            _collection.Clear();

        public bool Contains(T item) => 
            _collection.Contains(item);
        
        public IEnumerator<T> GetEnumerator() =>
            _collection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }
}