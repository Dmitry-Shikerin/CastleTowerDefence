using System;
using Sirenix.OdinInspector;
using Sources.Frameworks.GameServices.Cameras.Domain;
using Sources.Frameworks.GameServices.Cameras.Infrastructure.Services.Interfaces;
using Sources.Frameworks.GameServices.Singletones.Monobehaviours;
using Zenject;

namespace Sources.Frameworks.GameServices.Cameras.Presentation.Implementation
{
    public class RuntimeCameraService : MonoBehaviourSingleton<RuntimeCameraService>
    {
        private ICameraService _cameraService;

        [Button]
        public void PlayBattle() =>
            _cameraService.PlayDirector(DirectorId.Battle);

        [Inject]
        private void Construct(ICameraService cameraService) =>
            _cameraService = cameraService ?? throw new ArgumentNullException(nameof(cameraService));
    }
}