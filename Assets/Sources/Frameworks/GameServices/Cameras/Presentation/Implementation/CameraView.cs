using System.Collections.Generic;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using UnityEngine;

namespace Sources.Frameworks.GameServices.Cameras.Presentation.Implementation
{
    public class CameraView : View
    {
        [SerializeField] private List<VirtualCameraView> _cameras;
        
        public IReadOnlyList<VirtualCameraView> Cameras => _cameras;
    }
}