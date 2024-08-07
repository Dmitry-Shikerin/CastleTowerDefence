using System;
using System.Collections;
using System.Collections.Generic;

namespace MyAudios.Soundy.Utils
{
    public class CustomList<T> : IList<T>
    {
        private List<T> _collection = new List<T>();
        
        public event Action CollectionChanged;
        public event Action<T, int> ItemRemoved;
        public event Action ItemAdded;

        public int Count => _collection.Count;
        public bool IsReadOnly { get; set; }

        public T this[int index]
        {
            get => _collection[index];
            set => _collection[index] = value;
        }

        public IEnumerator<T> GetEnumerator() =>
            _collection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        public void Add(T item)
        {
            _collection.Add(item);
            CollectionChanged?.Invoke();
            ItemAdded?.Invoke();
        }

        public void Clear() =>
            _collection.Clear();

        public bool Contains(T item) =>
            _collection.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) =>
            _collection.CopyTo(array, arrayIndex);

        public bool Remove(T item)
        {
            ItemRemoved?.Invoke(item, _collection.IndexOf(item));

            return _collection.Remove(item);
        }

        public int IndexOf(T item) =>
            _collection.IndexOf(item);

        public void Insert(int index, T item) =>
            _collection.Insert(index, item);

        public void RemoveAt(int index) =>
            _collection.RemoveAt(index);
    }
}