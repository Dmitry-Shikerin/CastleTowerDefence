using Sources.Frameworks.GameServices.Scenes.Domain.Interfaces;

namespace Sources.Frameworks.GameServices.Scenes.Domain.Implementation
{
    public class ScenePayload : IScenePayload
    {
        public ScenePayload(string sceneId, bool canLoad, bool canFromGameplay)
        {
            SceneId = sceneId;
            CanLoad = canLoad;
            CanFromGameplay = canFromGameplay;
        }

        public string SceneId { get; }
        public bool CanLoad { get; }
        public bool CanFromGameplay { get; }
    }
}