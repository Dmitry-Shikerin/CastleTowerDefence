namespace Sources.DomainInterfaces.Models.Payloads
{
    public interface IScenePayload
    {
        string SceneId { get; }
        bool CanLoad { get; }
        bool CanFromGameplay { get; }
    }
}