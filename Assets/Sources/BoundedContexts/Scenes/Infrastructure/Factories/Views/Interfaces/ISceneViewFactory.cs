using Sources.Frameworks.GameServices.Scenes.Domain.Interfaces;

namespace Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views.Interfaces
{
    public interface ISceneViewFactory
    {
        public void Create(IScenePayload payload);
    }
}