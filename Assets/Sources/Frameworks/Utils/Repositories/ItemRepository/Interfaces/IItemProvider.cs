using System.Collections.Generic;

namespace Sources.Utils.Repositories.ItemRepository.Interfaces
{
    public interface IItemProvider<T1>
    {
        public int Count { get; }
        public IReadOnlyCollection<T1> Collection { get; }

        public T2 Get<T2>()
            where T2 : T1;

        public bool TryGetComponent<T2>(out T2 @object)
            where T2 : T1;
    }
}