using System;
using Sources.Frameworks.GameServices.Scenes.Domain.Implementation;
using Sources.Frameworks.GameServices.Scenes.Services.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sources.App.Core
{
    public class AppCore : MonoBehaviour
    {
        private ISceneService _sceneService;

        private void Awake() =>
            DontDestroyOnLoad(this);

        private async void Start()
        {
            try
            {
                await _sceneService.ChangeSceneAsync(
                    SceneManager.GetActiveScene().name,
                    new ScenePayload(SceneManager.GetActiveScene().name, false, false));
            }
            catch(ArgumentNullException)
            {
            }
            catch (OperationCanceledException)
            {
            }
        }

        private void Update() =>
            _sceneService.Update(Time.deltaTime);

        private void LateUpdate() =>
            _sceneService.UpdateLate(Time.deltaTime);

        private void FixedUpdate() =>
            _sceneService.UpdateFixed(Time.fixedDeltaTime);

        private void OnDestroy() =>
            _sceneService?.Disable();

        public void Construct(ISceneService sceneService) =>
            _sceneService = sceneService ?? throw new ArgumentNullException(nameof(sceneService));
    }
}