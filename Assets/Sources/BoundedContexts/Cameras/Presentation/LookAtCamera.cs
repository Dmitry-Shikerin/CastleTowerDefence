using Sources.Frameworks.GameServices.UpdateServices.Interfaces;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.Views.Constructors;
using UnityEngine;
using Zenject;

namespace Sources.BoundedContexts.Cameras.Presentation
{
    public class LookAtCamera : View, IConstruct<IUpdateRegister>
    {
        private Camera _mainCamera;
        private IUpdateRegister _updateRegister;

        private void Awake() =>
            _mainCamera = Camera.main;

        private void OnEnable()
        {
            if (_updateRegister == null)
                return;
            
            _updateRegister.UpdateChanged += OnUpdate;
        }

        private void OnDisable()
        {
            if (_updateRegister == null)
                return;
            
            _updateRegister.UpdateChanged -= OnUpdate;
        }

        private void OnUpdate(float deltaTime)
        {
            Quaternion rotation = _mainCamera.transform.rotation;
            transform.LookAt(transform.position + rotation * Vector3.back, rotation * Vector3.up);
        }

        [Inject]
        public void Construct(IUpdateRegister leaderBoardElementViews)
        {
            Hide();
            _updateRegister = leaderBoardElementViews;
            Show();
        }
    }
}