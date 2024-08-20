using Cysharp.Threading.Tasks;
using Sources.InfrastructureInterfaces.Services.SceneLoaderService;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Sources.Frameworks.GameServices.SceneLoaderServices.Implementation
{
    public class AddressableSceneLoaderService : ISceneLoaderService
    {
        private SceneInstance? _currentScene;
        
        public async UniTask Load(string sceneName)
        {
            if(_currentScene != null) 
                await Unload();
        
            _currentScene = await UnityEngine.AddressableAssets.Addressables.LoadSceneAsync(sceneName);
        }

        public async UniTask Unload()
        {
            if (_currentScene is not SceneInstance scene)
                return;
        
            await UnityEngine.AddressableAssets.Addressables.UnloadSceneAsync(scene);
        
            _currentScene = null;
        }
    }
}