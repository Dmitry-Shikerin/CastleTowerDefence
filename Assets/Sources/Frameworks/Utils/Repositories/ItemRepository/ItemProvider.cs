using System;
using System.Collections.Generic;
using Sources.Utils.Repositories.ItemRepository.Interfaces;

namespace Sources.Utils.Repositories.ItemRepository
{
    public class ItemProvider<T1> : IItemProvider<T1>
    {
        private readonly Dictionary<Type, T1> _repositoryes = new ();

        public int Count => _repositoryes.Count;
        public IReadOnlyCollection<T1> Collection => _repositoryes.Values;

        public T2 Get<T2>()
            where T2 : T1
        {
            if (_repositoryes.ContainsKey(typeof(T2)) == false)
                throw new InvalidOperationException();

            if (_repositoryes[typeof(T2)] is T2 concrete)
                return concrete;

            throw new InvalidOperationException(nameof(T2));
        }

        public bool TryGetComponent<T2>(out T2 @object)
            where T2 : T1
        {
            if (_repositoryes.ContainsKey(typeof(T2)) == false)
            {
                @object = default;
                return false;
            }

            if (_repositoryes[typeof(T2)] is T2 concrete)
            {
                @object = concrete;
                return true;
            }

            @object = default;

            return false;
        }

        public void Remove<T2>()
            where T2 : T1
        {
            if (_repositoryes.ContainsKey(typeof(T2)) == false)
                throw new InvalidOperationException();

            if (_repositoryes[typeof(T2)] is not T2)
                throw new InvalidOperationException(nameof(T2));

            _repositoryes.Remove(typeof(T2));
        }

        public void Add<T2>(T2 @object)
            where T2 : T1
        {
            if (_repositoryes.ContainsKey(typeof(T2)))
                throw new InvalidOperationException();

            _repositoryes[typeof(T2)] = @object;
        }

        public void AddCollection(IEnumerable<T1> items)
        {
            foreach (var item in items)
            {
                var type = item.GetType();

                if (_repositoryes.ContainsKey(type))
                    throw new InvalidOperationException();

                _repositoryes[type] = item;
            }
        }
    }
}