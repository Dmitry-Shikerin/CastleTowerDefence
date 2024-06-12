using Sources.BoundedContexts.Scenes.Domain;

namespace Sources.BoundedContexts.Scenes.Infrastructure.Factories.Domain.Interfaces
{
    public interface IGameplayModelsLoaderService
    {
        GameplayModel Load();
    }
}