using Sources.BoundedContexts.Scenes.Domain;

namespace Sources.Frameworks.GameServices.Scenes.Infrastructure.Factories.Domain.Interfaces
{
    public interface IGameplayModelsLoaderService
    {
        GameplayModel Load();
    }
}