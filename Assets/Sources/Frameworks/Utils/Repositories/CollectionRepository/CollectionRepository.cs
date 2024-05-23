using System;
using System.Collections.Generic;
using System.Linq;
using Sources.Utils.Repositories.CollectionRepository.Containers;
using Sources.Utils.Repositories.CollectionRepository.ContainersInterfaces;

namespace Sources.Utils.Repositories.CollectionRepository
{
    public class CollectionRepository
    {
        private readonly Dictionary<Type, ICollectionContainer> _repositoryes = new ();

        public IReadOnlyList<T> Get<T>()
        {
            if (_repositoryes.ContainsKey(typeof(T)) == false)
                throw new InvalidOperationException();

            if (_repositoryes[typeof(T)] is not CollectionContainerGeneric<T> concrete)
                throw new InvalidOperationException(nameof(T));

            return concrete.Get().ToList();
        }

        public void Add<T>(IEnumerable<T> objects)
        {
            if (_repositoryes.ContainsKey(typeof(T)))
                throw new InvalidOperationException();

            var containerGenericCollection =
                new CollectionContainerGeneric<T>();
            containerGenericCollection.Set(objects);

            _repositoryes[typeof(T)] = containerGenericCollection;
        }
    }
}