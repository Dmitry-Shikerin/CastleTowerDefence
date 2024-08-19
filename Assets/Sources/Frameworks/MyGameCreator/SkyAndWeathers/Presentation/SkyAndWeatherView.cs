using Sirenix.OdinInspector;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using UnityEngine;

namespace Sources.Frameworks.MyGameCreator.SkyAndWeathers.Presentation
{
    [RequireComponent(typeof(Light))]
    public class SkyAndWeatherView : View
    {
        [Required] [SerializeField] private Light _light;
        [Required] [SerializeField] private RainView _rainView;
        
        public Light Light => _light;

        [OnInspectorInit]
        private void SetLight()
        {
            if (_light != null)
                return;
            
            _light = GetComponent<Light>();
        }
    }
}