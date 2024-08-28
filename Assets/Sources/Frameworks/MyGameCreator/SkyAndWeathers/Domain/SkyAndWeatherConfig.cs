using Sirenix.OdinInspector;
using Sources.Frameworks.GameServices.ConfigCollectors;
using Sources.Frameworks.GameServices.ConfigCollectors.Domain.ScriptableObjects;
using UnityEngine;

namespace Sources.Frameworks.MyGameCreator.SkyAndWeathers.Domain
{
    public class SkyAndWeatherConfig : Config
    {
        [SerializeField] private Color _upperColor;
        [SerializeField] private Color _middleColor;
        [SerializeField] private Color _lowerColor;
        
        public Color UpperColor => _upperColor;
        public Color MiddleColor => _middleColor;
        public Color LowerColor => _lowerColor;
    }
}