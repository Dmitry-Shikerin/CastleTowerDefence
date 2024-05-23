namespace Sources.Utils.Repositories.CollectionRepository.Containers
{
    public class InstanceContainer<T> : IInstanceContainer
    {
        public T Instance { get ; set ; }
    }
}