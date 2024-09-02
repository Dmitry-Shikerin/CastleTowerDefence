namespace Sources.Frameworks.GameServices.Scenes.Domain.Interfaces
{
    public interface IScenePayload
    {
        string SceneId { get; }
        bool CanLoad { get; }
        bool CanFromGameplay { get; }
    }
}