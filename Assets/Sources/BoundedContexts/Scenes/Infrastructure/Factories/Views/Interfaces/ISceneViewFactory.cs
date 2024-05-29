using Sources.DomainInterfaces.Models.Payloads;

namespace Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views.Interfaces
{
    public interface ISceneViewFactory
    {
        public void Create(IScenePayload payload);
    }
}